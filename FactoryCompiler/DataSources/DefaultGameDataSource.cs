using System.Threading.Tasks;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.DataSources;

public class DefaultGameDataSource : ISource<LoadResult<IGameData>>
{
    public async Task<LoadResult<IGameData>> Load()
    {
        return new LoadResult<IGameData>(null)
        {
            Success = true,
            Asset = new DefaultGameData().Build(),
            Diagnostics =
            {
                Diagnostic.Info("Loaded default game data."),
            },
        };
    }
}
