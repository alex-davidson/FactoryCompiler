using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FactoryCompiler.DataSources;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.Jobs.Visualise;

public class SourceDataModel
{
    public bool IgnoreErrors { get; init; }
    public LoadResult<IGameData> GameData { get; init; } = LoadResult<IGameData>.Empty;
    public ImmutableList<LoadResult<FactoryDescription?>> FactoryParts { get; init; } = ImmutableList<LoadResult<FactoryDescription?>>.Empty;
    public FactoryDescription FactoryDescription { get; init; } = null!;

    public bool IsFailed
    {
        get
        {
            if (IgnoreErrors) return false;
            if (GameData?.Success != true) return true;
            if (!FactoryParts.All(x => x.Success)) return true;
            return false;
        }
    }
}

public static class SourceDataModelExtensions
{
    public static IEnumerable<Diagnostic> GetOrderedDiagnostics(this SourceDataModel? model)
    {
        if (model == null) return Enumerable.Empty<Diagnostic>();
        return model.GameData.Diagnostics
            .Concat(model.FactoryParts.SelectMany(x => x.Diagnostics));
    }
}
