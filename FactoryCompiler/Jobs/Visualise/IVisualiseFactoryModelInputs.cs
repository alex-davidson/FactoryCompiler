using System.Collections.Immutable;

namespace FactoryCompiler.Jobs.Visualise;

public record VisualiseFactoryModelInputs
{
    public static VisualiseFactoryModelInputs None => new VisualiseFactoryModelInputs();

    public string? DatabaseFilePath { get; init; }
    public ImmutableArray<string> FactoryDescriptionFilePaths { get; init; } = ImmutableArray<string>.Empty;
    public bool IgnoreErrors { get; init; }
}
