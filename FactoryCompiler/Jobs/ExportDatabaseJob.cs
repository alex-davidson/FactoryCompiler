using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FactoryCompiler;
using FactoryCompiler.Jobs;
using FactoryCompiler.Model;

namespace FactoryCompiler.Jobs;

internal class ExportDatabaseJob
{
    public Stream Output { get; set; } = Stream.Null;

    public async Task<int> Run(CancellationToken token)
    {
        var gameData = GameDataBuilder.GetDefaultGameData();
        new GameDataSerialiser().Serialise(Output, gameData, true);
        return 0;
    }
}
