using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FactoryCompiler.DataSources;
using FactoryCompiler.Jobs.Visualise;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;
using Microsoft.Msagl.Drawing;
using static FactoryCompiler.Model.Dto;
using FactoryDescription = FactoryCompiler.Model.FactoryDescription;

namespace FactoryCompiler.Jobs
{
    internal class VisualiseJob
    {
        public string? DatabaseFilePath { get; set; }
        public ICollection<string> FactoryDescriptionFilePaths { get; } = new List<string>();
        public bool IgnoreErrors { get; set; }

        public async Task<int> Run(CancellationToken token)
        {
            var model = new VisualiseFactoryModel();

            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var thread = new Thread(() => RunWpf(model, tcs));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            await RunModel(model, token);

            await tcs.Task;
            thread.Join();
            return 0;
        }

        private async Task RunModel(VisualiseFactoryModel model, CancellationToken token)
        {
            model.SourceData = await LoadSourceData(token);

            // Bail out if failure is fatal.
            if (model.SourceData.IsFailed) return;

            model.Factory = await CalculateFactoryState(model.SourceData);

            if (model.Factory.IsFailed) return;

            model.Graph = await BuildFactoryGraph(model.Factory);
        }

        private async Task<SourceDataModel> LoadSourceData(CancellationToken token)
        {
            ISource<LoadResult<IGameData>> gameDataSource = DatabaseFilePath == null
                ? new DefaultGameDataSource()
                : new GameDataSource(Path.GetFullPath(DatabaseFilePath));
            var gameData = await gameDataSource.Load();
            var factoryParts = new List<LoadResult<FactoryDescription?>>();
            foreach (var sourcePath in FactoryDescriptionFilePaths)
            {
                token.ThrowIfCancellationRequested();
                var source = new FactoryDescriptionDataSource(Path.GetFullPath(sourcePath));
                factoryParts.Add(await source.Load());
            }
            var factoryDescription = FactoryDescription.Merge(factoryParts.Where(x => x.Asset != null).Select(x => x.Asset!));
            return new SourceDataModel
            {
                IgnoreErrors = IgnoreErrors,
                GameData = gameData,
                FactoryParts = factoryParts.ToImmutableList(),
                FactoryDescription = factoryDescription,
            };
        }

        private async Task<Graph?> BuildFactoryGraph(FactoryModel modelFactory)
        {
            return new FactoryGraphBuilder().Build(modelFactory.State);
        }

        private async Task<FactoryModel> CalculateFactoryState(SourceDataModel sourceData)
        {
            var stateFactory = new FactoryStateFactory(sourceData.GameData!.Asset);
            var state = stateFactory.Create(sourceData.FactoryDescription);
            // Bail out if failure is fatal.
            if (!IgnoreErrors && state.HasErrors)
            {
                return new FactoryModel(state, null, IgnoreErrors);
            }
            new FactoryStateEvaluator().UpdateInPlace(state);

            // Summarise:
            var summary = new FactoryStateSummariser().Summarise(state);
            return new FactoryModel(state, summary, IgnoreErrors);
        }

        private void RunWpf(VisualiseFactoryModel model, TaskCompletionSource tcs)
        {
            try
            {
                var app = new Application();
                // Native.ShowWindow(Native.GetConsoleWindow(), 0 /*SW_HIDE*/);
                app.Run(new MainWindow(model));
                tcs.TrySetResult();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }
    }

    internal static class Native
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
