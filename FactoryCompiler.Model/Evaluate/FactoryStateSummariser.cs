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
        var tradeByRegion = new Dictionary<Region, ItemVolumesState>();

        foreach (var region in state.Regions.OrderBy(x => x.RegionName.Name))
        {
            // Summarise the regions excess and shortfall of items which are *not* imported or exported.
            var transported = linksByRegion[region.Definition].ToDictionary(x => x.Item, x => x.Direction);
            var withoutTransport = ItemVolumesState.Sum(region.ItemVolumes.Where(x => !IsExternalised(x)));

            summary.Regions.Add(region.Definition, withoutTransport);
            tradeByRegion.Add(region.Definition, ItemVolumesState.Sum(region.ItemVolumes.Where(IsExternalised)));

            bool IsExternalised(ItemVolume itemVolume)
            {
                if (!transported.TryGetValue(itemVolume.Item, out var direction)) return false;
                // Excess is marked for export?
                if (direction == TransportLinkDirection.FromRegion && itemVolume.Volume > 0) return true;
                // Shortfall is marked for import?
                if (direction == TransportLinkDirection.ToRegion && itemVolume.Volume < 0) return true;
                return false;
            }
        }

        var linksByNetwork = state.TransportLinks.ToLookup(x => x.NetworkName);
        foreach (var network in linksByNetwork.OrderBy(x => x.Key.Name))
        {
            var networkState = new ItemVolumesState();
            foreach (var link in network)
            {
                if (!tradeByRegion.TryGetValue(link.Region, out var trade)) continue;
                // Volumes sent to the region are already negative.
                networkState.Produce(link.Item, trade.GetVolume(link.Item));
            }

            summary.Networks.Add(network.Key, networkState);
        }
        return summary;
    }
}
