using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using FactoryCompiler.DataSources;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;
using Microsoft.Msagl.Drawing;

namespace FactoryCompiler.Jobs.Visualise;

public class RefreshVisualiseFactoryModelCommand : ICommand
{
    private readonly VisualiseFactoryModelInputs inputs;
    private readonly VisualiseFactoryModel model;

    public RefreshVisualiseFactoryModelCommand(VisualiseFactoryModelInputs inputs, VisualiseFactoryModel model)
    {
        this.inputs = inputs;
        this.model = model;
        model.PropertyChanged += Model_PropertyChanged;
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(model.IsRefreshing)) CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter) => !model.IsRefreshing;

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        _ = RefreshModel(CancellationToken.None);
    }

    public event EventHandler? CanExecuteChanged;

    public async Task RefreshModel(CancellationToken token)
    {
        if (model.IsRefreshing) return;
        model.IsRefreshing = true;
        try
        {
            model.SourceData = null;
            model.Factory = null;
            model.Graph = null;

            model.SourceData = await LoadSourceData(inputs, token);

            // Bail out if failure is fatal.
            if (model.SourceData.IsFailed) return;

            model.Factory = await CalculateFactoryState(inputs, model.SourceData);

            if (model.Factory.IsFailed) return;

            model.Graph = await BuildFactoryGraph(model.Factory);
        }
        finally
        {
            model.IsRefreshing = false;
        }
    }

    private static async Task<SourceDataModel> LoadSourceData(VisualiseFactoryModelInputs inputs, CancellationToken token)
    {
        ISource<LoadResult<IGameData>> gameDataSource = inputs.DatabaseFilePath == null
            ? new DefaultGameDataSource()
            : new GameDataSource(Path.GetFullPath(inputs.DatabaseFilePath));
        var gameData = await gameDataSource.Load();
        var factoryParts = new List<LoadResult<FactoryDescription?>>();
        foreach (var sourcePath in inputs.FactoryDescriptionFilePaths)
        {
            token.ThrowIfCancellationRequested();
            var source = new FactoryDescriptionDataSource(Path.GetFullPath(sourcePath));
            factoryParts.Add(await source.Load());
        }
        var factoryDescription = FactoryDescription.Merge(factoryParts.Where(x => x.Asset != null).Select(x => x.Asset!));
        return new SourceDataModel
        {
            IgnoreErrors = inputs.IgnoreErrors,
            GameData = gameData,
            FactoryParts = factoryParts.ToImmutableList(),
            FactoryDescription = factoryDescription,
        };
    }

    private static async Task<Graph?> BuildFactoryGraph(FactoryModel modelFactory)
    {
        return new FactoryGraphBuilder().Build(modelFactory.State);
    }

    private static async Task<FactoryModel> CalculateFactoryState(VisualiseFactoryModelInputs inputs, SourceDataModel sourceData)
    {
        var stateFactory = new FactoryStateFactory(sourceData.GameData!.Asset);
        var state = stateFactory.Create(sourceData.FactoryDescription);
        // Bail out if failure is fatal.
        if (!inputs.IgnoreErrors && state.HasErrors)
        {
            return new FactoryModel(state, null, inputs.IgnoreErrors);
        }
        new FactoryStateEvaluator().UpdateInPlace(state);

        // Summarise:
        var summary = new FactoryStateSummariser().Summarise(state);
        return new FactoryModel(state, summary, inputs.IgnoreErrors);
    }
}
