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
            var inputs = new VisualiseFactoryModelInputs
            {
                DatabaseFilePath = DatabaseFilePath,
                FactoryDescriptionFilePaths = FactoryDescriptionFilePaths.ToImmutableArray(),
                IgnoreErrors = IgnoreErrors,
            };
            var model = new VisualiseFactoryModel(inputs);

            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var thread = new Thread(() => RunWpf(model, tcs));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            await model.RefreshCommand.RefreshModel(token);

            await tcs.Task;
            thread.Join();
            return 0;
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
