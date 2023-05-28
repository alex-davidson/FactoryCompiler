using System.Collections.Generic;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Model.Evaluate;

public class FactoryStateSummary
{
    /// <summary>
    /// Internal excess and shortfall for each region.
    /// </summary>
    public IDictionary<Region, IItemVolumesState> Regions { get; } = new Dictionary<Region, IItemVolumesState>();

    public IItemVolumesState GetRegion(Region region) => Regions.TryGetValue(region, out var volume) ? volume : ItemVolumesState.Empty;

    /// <summary>
    /// Group contributions to the excess and shortfall for each region.
    /// </summary>
    public IDictionary<Group, GroupSummary> Groups { get; } = new Dictionary<Group, GroupSummary>();

    public GroupSummary GetGroup(Group group) => Groups.TryGetValue(group, out var summary) ? summary : new GroupSummary(ItemVolumesState.Empty, ItemVolumesState.Empty);

    /// <summary>
    /// Excess and shortfall for each network.
    /// </summary>
    public IDictionary<Identifier, IItemVolumesState> Networks { get; } = new Dictionary<Identifier, IItemVolumesState>();

    public IItemVolumesState GetNetwork(Identifier networkName) => Networks.TryGetValue(networkName, out var volume) ? volume : ItemVolumesState.Empty;

    /// <summary>
    /// Imports and exports for each transport link.
    /// </summary>
    public IDictionary<TransportLinkAggregate, IItemVolumesState> TransportLinks { get; } = new Dictionary<TransportLinkAggregate, IItemVolumesState>();

    public IItemVolumesState GetTransportLink(TransportLinkAggregate linkAggregate) => TransportLinks.TryGetValue(linkAggregate, out var volume) ? volume : ItemVolumesState.Empty;
}
