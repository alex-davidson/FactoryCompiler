using System.Collections.Generic;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.DataSources;

public class LoadResult<T>
{
    public LoadResult(string? source)
    {
        Source = source;
    }

    public string? Source { get; }
    /// <summary>
    /// True if the asset is believed to have been loaded in a usable state.
    /// </summary>
    public bool Success { get; set; } = false;
    /// <summary>
    /// The loaded asset. May not be usable if Success is false.
    /// </summary>
    public T Asset { get; set; } = default!;
    /// <summary>
    /// List of validation failures (warnings and errors) encountered during loading.
    /// </summary>
    public IList<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

    public static LoadResult<T> Empty => new LoadResult<T>(null);
}
