using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FactoryCompiler.Jobs;
using McMaster.Extensions.CommandLineUtils;

namespace FactoryCompiler;

internal class Program
{
    internal static async Task<int> Main(string[] args)
    {
        var app = new CommandLineApplication();
        app.Name = Assembly.GetExecutingAssembly().GetName().Name;
        app.Description = "Provides visualisations for Satisfactory factories described by JSON files.";

        app.HelpOption("-?|-h|--help", true);

        app.Command("summarise", c =>
        {
            c.Description = "Write to STDOUT a summary of excesses and shortfalls, by region.";

            var dbOption = c.Option("--db <db.json>", "JSON file containing recipes, etc from the game. The internal file will be used if not specified.", CommandOptionType.SingleValue);
            var requestOption = c.Argument("<factory.json> ...", "One or more JSON files describing the factory.", true);
            var ignoreErrorsOption = c.Option<bool>("--ignore-errors", "Try to continue when an error occurs.", CommandOptionType.NoValue);

            c.OnExecuteAsync(async token => {
                var job = new SummariseJob
                {
                    DatabaseFilePath = dbOption.Value(),
                    IgnoreErrors = ignoreErrorsOption.ParsedValue,
                    Output = Console.OpenStandardOutput(),
                    Error = Console.Error,
                };
                foreach (var path in requestOption.Values)
                {
                    if (!string.IsNullOrEmpty(path)) job.FactoryDescriptionFilePaths.Add(path);
                }
                return await job.Run(token);
            });
        });

        app.Command("visualise", c =>
        {
            c.Description = "Show an interactive visualisation of the factory.";

            var dbOption = c.Option("--db <db.json>", "JSON file containing recipes, etc from the game. The internal file will be used if not specified.", CommandOptionType.SingleValue);
            var requestOption = c.Argument("<factory.json> ...", "One or more JSON files describing the factory.", true);
            var ignoreErrorsOption = c.Option<bool>("--ignore-errors", "Try to continue when an error occurs.", CommandOptionType.NoValue);

            c.OnExecuteAsync(async token => {
                var job = new VisualiseJob
                {
                    DatabaseFilePath = dbOption.Value(),
                    IgnoreErrors = ignoreErrorsOption.ParsedValue,
                };
                foreach (var path in requestOption.Values)
                {
                    if (!string.IsNullOrEmpty(path)) job.FactoryDescriptionFilePaths.Add(path);
                }
                return await job.Run(token);
            });
        });

        app.Command("export-db", c =>
        {
            c.Description = "Write out the application's internal database of recipes, etc as JSON. This is the default database used by other commands if --db is not specified.";

            var targetOption = c.Argument("<db.json>", "Path of the file which should be written. Use - to specify STDOUT.").IsRequired();

            c.OnExecuteAsync(async token => {
                await using (var writer = OpenOutput(targetOption.Value!, false))
                {
                    var job = new ExportDatabaseJob
                    {
                        Output = writer,
                    };
                    return await job.Run(token);
                }
            });
        });
        app.Command("create-example-factory", c =>
        {
            c.Description = "Write out an example factory description file as JSON.";

            var targetOption = c.Argument("<factory.json>", "Path of the file which should be written. Use - to specify STDOUT.").IsRequired();

            c.OnExecuteAsync(async token => {
                await using (var writer = OpenOutput(targetOption.Value!, false))
                {
                    var job = new CreateExampleFactoryJob
                    {
                        Output = writer,
                    };
                    return await job.Run(token);
                }
            });
        });

        app.OnExecute(() =>
        {
            app.Error.WriteLine("Specify a command.");
            app.ShowHelp();
            return 1;
        });

        return await app.ExecuteAsync(args);
    }

    private static Stream OpenOutput(string filePath, bool overwrite = false)
    {
        if (filePath == "-") return Console.OpenStandardOutput();
        var fullFilePath = Path.GetFullPath(filePath);
        return new FileStream(fullFilePath, new FileStreamOptions
        {
            Access = FileAccess.Write,
            Mode = overwrite ? FileMode.Create : FileMode.CreateNew,
            Share = FileShare.None,
        });
    }
}
