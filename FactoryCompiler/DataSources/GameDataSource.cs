using System;
using System.IO;
using System.Threading.Tasks;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.DataSources;

public class GameDataSource : ISource<LoadResult<IGameData>>
{
    private readonly string fullPath;

    public GameDataSource(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(fullPath));
        if (!Path.IsPathRooted(fullPath)) throw new ArgumentException($"Not an absolute path: {fullPath}");
        this.fullPath = fullPath;
    }

    public async Task<LoadResult<IGameData>> Load()
    {
        var result = new LoadResult<IGameData>(fullPath);
        try
        {
            await using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                result.Success = new GameDataSerialiser().TryDeserialise(stream, out var gameData, result.Diagnostics);
                if (gameData == null)
                {
                    result.Diagnostics.Add(Diagnostic.Warning("Default game data will be used instead."));
                    result.Asset = new DefaultGameData().Build();
                }
                else
                {
                    result.Asset = gameData;
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            result.Diagnostics.Add(Diagnostic.Error("File not found.", ex));
            result.Diagnostics.Add(Diagnostic.Warning("Default game data will be used instead."));
            result.Asset = new DefaultGameData().Build();
        }
        return result;
    }
}
