using FactoryCompiler.Model.State;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FactoryCompiler.Model.Algorithms;

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
            var transported = linksByRegion[region.Definition]
                .Select(x => new { x.Item, x.Direction })
                .Distinct()
                .ToDictionary(x => x.Item, x => x.Direction);
            var withoutTransport = ItemVolumesState.Sum(region.ItemVolumes.Where(x => !IsExternalised(transported, x)));

            summary.Regions.Add(region.Definition, withoutTransport);
            tradeByRegion.Add(region.Definition, ItemVolumesState.Sum(region.ItemVolumes.Where(x => IsExternalised(transported, x))));

            SummariseGroups(summary, region.Groups, region.ItemVolumes, transported);
        }

        // Cannot process networks independently, as many-to-many relationships may exist between networks and regions.
        // Must process per-item-type and work with subgraphs of regions and networks.
        // We mostly assume that regions can never export what they import, so each subgraph should be representable
        // in three layers: exporting regions, networks, importing regions. However the user can specify IgnoreErrors,
        // in which case we could be dealing with just about anything here.
        var analyser = new TransportGraphAnalyser();
        var distribution = state.TransportLinks
            .GroupBy(x => x.Item)
            .SelectMany(s => analyser.SolveDistribution(s.ToList(), s.Key, tradeByRegion))
            .ToArray();

        foreach (var linkAggregate in distribution.GroupBy(x => new TransportLinkAggregate(x.Link.Direction, x.Link.Region, x.Link.NetworkName), x => x.ItemVolume))
        {
            summary.TransportLinks.Add(linkAggregate.Key, ItemVolumesState.Sum(linkAggregate));
        }
        foreach (var networkAggregate in distribution.GroupBy(x => x.Link.NetworkName, x => x.ItemVolume))
        {
            summary.Networks.Add(networkAggregate.Key, ItemVolumesState.Sum(networkAggregate));
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
