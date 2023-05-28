using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.DataSources;

public class FactoryDescriptionDataSource : ISource<LoadResult<FactoryDescription?>>
{
    private readonly string fullPath;

    public FactoryDescriptionDataSource(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(fullPath));
        if (!Path.IsPathRooted(fullPath)) throw new ArgumentException($"Not an absolute path: {fullPath}");
        this.fullPath = fullPath;
    }

    public async Task<LoadResult<FactoryDescription?>> Load()
    {
        var result = new LoadResult<FactoryDescription?>(fullPath);
        try
        {
            await using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                result.Success = new FactoryDescriptionSerialiser().TryDeserialise(stream, out var description, result.Diagnostics);
                result.Asset = description;
            }
        }
        catch (FileNotFoundException ex)
        {
            result.Diagnostics.Add(Diagnostic.Error("File not found.", ex, fullPath));
        }
        catch (JsonException ex)
        {
            result.Diagnostics.Add(Diagnostic.Error("File appears corrupt.", ex, fullPath));
        }
        catch (Exception ex)
        {
            result.Diagnostics.Add(Diagnostic.Error("Unknown error occurred.", ex, fullPath));
        }
        return result;
    }
}
