using System.Threading.Tasks;

namespace FactoryCompiler.DataSources;

/// <summary>
/// Contract of an asynchronous data source.
/// </summary>
/// <remarks>
/// These may be used by the CLI tools as well as GUI visualisations.
/// </remarks>
public interface ISource<T>
{
    Task<T> Load();
}
