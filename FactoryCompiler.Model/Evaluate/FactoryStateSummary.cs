using System.Collections.Generic;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Model.Evaluate;

public class FactoryStateSummary
{
    /// <summary>
    /// Internal excess and shortfall for each region.
    /// </summary>
    public IDictionary<Region, ItemVolumesState> Regions { get; } = new Dictionary<Region, ItemVolumesState>();

    /// <summary>
    /// Excess and shortfall for each network.
    /// </summary>
    public IDictionary<Identifier, ItemVolumesState> Networks { get; } = new Dictionary<Identifier, ItemVolumesState>();
}
