using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            var app = new WpfAppThread(app => app.Run(new MainWindow(model)));
            app.Start();

            await app.Dispatch(() => model.RefreshCommand.RefreshModel(token));

            await app.WaitForShutdown();
            return 0;
        }
    }
}
