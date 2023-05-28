using FactoryCompiler.Model.State;
using System.Collections.Generic;
using System.Linq;

namespace FactoryCompiler.Model.Evaluate;

public class FactoryStateSummariser
{
    public FactoryStateSummary Summarise(FactoryState state)
    {
        var summary = new FactoryStateSummary();
        var linksByRegion = state.TransportLinks.ToLookup(x => x.Region);
        var tradeByRegion = new Dictionary<Region, IItemVolumesState>();

        foreach (var region in state.Regions.OrderBy(x => x.RegionName.Name))
        {
            // Summarise the regions excess and shortfall of items which are *not* imported or exported.
            var transported = linksByRegion[region.Definition].ToDictionary(x => x.Item, x => x.Direction);
            var withoutTransport = ItemVolumesState.Sum(region.ItemVolumes.Where(x => !IsExternalised(transported, x)));

            summary.Regions.Add(region.Definition, withoutTransport);
            tradeByRegion.Add(region.Definition, ItemVolumesState.Sum(region.ItemVolumes.Where(x => IsExternalised(transported, x))));

            SummariseGroups(summary, region.Groups, region.ItemVolumes, transported);
        }

        var linksByNetwork = state.TransportLinks.ToLookup(x => x.NetworkName);
        foreach (var network in linksByNetwork.OrderBy(x => x.Key.Name))
        {
            var networkState = new List<ItemVolume>();
            foreach (var link in network.GroupBy(x => new { x.Region, x.Direction }))
            {
                if (!tradeByRegion.TryGetValue(link.Key.Region, out var trade)) continue;
                var linkState = ItemVolumesState.Sum(link.Select(x => trade.GetVolume(x.Item)));
                summary.TransportLinks.Add(new TransportLinkAggregate(link.Key.Direction, link.Key.Region, network.Key), linkState);
                networkState.AddRange(linkState);
            }

            // Volumes sent to the region are already negative.
            summary.Networks.Add(network.Key, ItemVolumesState.Sum(networkState));
        }
        return summary;
    }

    private static bool IsExternalised(Dictionary<Item, TransportLinkDirection> transported, ItemVolume itemVolume)
    {
        if (!transported.TryGetValue(itemVolume.Item, out var direction)) return false;
        // Excess is marked for export?
        if (direction == TransportLinkDirection.FromRegion && itemVolume.Volume > 0) return true;
        // Shortfall is marked for import?
        if (direction == TransportLinkDirection.ToRegion && itemVolume.Volume < 0) return true;
        return false;
    }

    private void SummariseGroups(FactoryStateSummary summary, IReadOnlyCollection<GroupState> groups, ItemVolumesState regionItemVolumes, Dictionary<Item, TransportLinkDirection> transported)
    {
        foreach (var group in groups)
        {
            var contributions = ItemVolumesState.Sum(group.ItemVolumes.GetNetVolumes().Where(x => !IsExternalised(transported, x)));
            summary.Groups.Add(group.Definition, new GroupSummary(regionItemVolumes, contributions));
            SummariseGroups(summary, group.Groups, regionItemVolumes, transported);
        }
    }
}
