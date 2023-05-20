using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FactoryCompiler.DataSources;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs;

internal class SummariseJob
{
    public string? DatabaseFilePath { get; set; }
    public ICollection<string> FactoryDescriptionFilePaths { get; } = new List<string>();
    public bool IgnoreErrors { get; set; }
    public Stream Output { get; set; } = Stream.Null;
    public TextWriter Error { get; set; } = TextWriter.Null;

    public async Task<int> Run(CancellationToken token)
    {
        var error = false;

        ISource<LoadResult<IGameData>> gameDataSource = DatabaseFilePath == null ? new DefaultGameDataSource() : new GameDataSource(Path.GetFullPath(DatabaseFilePath));

        var gameData = await gameDataSource.Load();
        WriteDiagnostics(gameData);
        if (!gameData.Success) error = true;

        var descriptionParts = new List<FactoryDescription>();
        foreach (var sourcePath in FactoryDescriptionFilePaths)
        {
            token.ThrowIfCancellationRequested();
            var source = new FactoryDescriptionDataSource(Path.GetFullPath(sourcePath));
            var descriptionPart = await source.Load();
            WriteDiagnostics(descriptionPart);
            if (descriptionPart.Asset != null) descriptionParts.Add(descriptionPart.Asset);
            if (!descriptionPart.Success) error = true;
        }

        // If we have errors and haven't been told to ignore them, bail out now that
        // we've explored as much of our input as possible.
        if (error && !IgnoreErrors)
        {
            return 2;
        }

        var description = FactoryDescription.Merge(descriptionParts);
        if (!description.Regions.Any())
        {
            Error.WriteLine("No regions loaded. Nothing to do.");
            return 0;
        }

        var stateFactory = new FactoryStateFactory(gameData.Asset!);
        var state = stateFactory.Create(description);

        if (state.Diagnostics.Any())
        {
            WriteDiagnostics("Construct factory", state.Diagnostics);
        }
        if (!IgnoreErrors && state.Diagnostics.Any(x => x.Severity >= Severity.Error)) return 2;

        if (!state.Regions.Any())
        {
            Error.WriteLine("No valid regions constructed. Nothing to do.");
            return 0;
        }

        new FactoryStateEvaluator().UpdateInPlace(state);

        var summary = new FactoryStateSummariser().Summarise(state);
        await using (var writer = new StreamWriter(Output))
        {
            new FactoryStateFormatter(writer).Format(summary);
        }

        return 0;
    }

    private void WriteDiagnostics<T>(LoadResult<T> result) =>
        WriteDiagnostics(result.Source, result.Diagnostics);

    private void WriteDiagnostics(string? source, ICollection<Diagnostic> diagnostics)
    {
        var errorCount = diagnostics.Count(x => x.Severity >= Severity.Error);
        var warningCount = diagnostics.Count(x => x.Severity == Severity.Warning);
        var context = source == null ? "" : source + ": ";
        Error.WriteLine($"{context}: Loaded with {warningCount} warnings and {errorCount} errors.");
        foreach (var diagnostic in diagnostics)
        {
            switch (diagnostic.Severity)
            {
                case Severity.Debug:
                case Severity.Info:
                    break;
                case Severity.Warning:
                case Severity.Error:
                case Severity.Fatal:
                    Error.Write(" * ");
                    Error.WriteLine(diagnostic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
