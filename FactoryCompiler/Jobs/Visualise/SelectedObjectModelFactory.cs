using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Mappers;
using FactoryCompiler.Model.State;
using Microsoft.Msagl.Drawing;
using Rationals;

namespace FactoryCompiler.Jobs.Visualise;

public class SelectedObjectModelFactory
{
    private readonly FactoryModel? factory;
    private readonly Graph? graph;

    public SelectedObjectModelFactory(FactoryModel? factory, Graph? graph)
    {
        this.factory = factory;
        this.graph = graph;
    }

    public SelectedObjectModel Create(DrawingObject? obj)
    {
        switch (obj?.UserData)
        {
            case Maybe<Region> { Exists: true, Value: var region }:
                return CreateSelectedRegion(obj, region);
            case Maybe<GroupState> { Exists: true, Value: var groupState }:
                return groupState.Production != null ? CreateSelectedProduction(obj, groupState) : CreateSelectedGroup(obj, groupState);
            case Maybe<Identifier> { Exists: true, Value: var network }:
                return CreateSelectedNetwork(obj, network);
            case Maybe<TransportLinkAggregate> { Exists: true, Value: var linkAggregate }:
                return CreateSelectedTransportLink(obj, linkAggregate);

            default:
                return SelectedObjectModel.None;
        }
    }

    private SelectedObjectModel CreateSelectedRegion(DrawingObject obj, Region region)
    {
        var connections = factory?.State.TransportLinks.Where(x => x.Region == region).ToArray() ?? Array.Empty<TransportLink>();
        var importsVia = connections.Where(x => x.Direction == TransportLinkDirection.ToRegion);
        var exportsVia = connections.Where(x => x.Direction == TransportLinkDirection.FromRegion);
        return new SelectedObjectModel
        {
            Object = obj,
            SortedItemVolumes = GetSortedItemVolumes(factory?.Summary?.GetRegion(region))
                .Select(x => new FormattedItemVolume(x.Item.Identifier.Name, x.Volume, ItemVolumeColourScheme.Area, false))
                .ToArray(),
            Description =
            {
                { "Region", region.RegionName.Name },
                { "Imports via", string.Join(", ", importsVia.Select(x => x.NetworkName.Name).Distinct()) },
                { "Exports via", string.Join(", ", exportsVia.Select(x => x.NetworkName.Name).Distinct()) },
            },
        };
    }

    private SelectedObjectModel CreateSelectedGroup(DrawingObject obj, GroupState groupState)
    {
        var region = graph?.GetNodeContainment(obj as Node)
            .Select(x => x?.UserData)
            .OfType<Maybe<Region>>()
            .Where(x => x.Exists)
            .Select(x => x.Value)
            .LastOrDefault();
        var regionName = region?.RegionName.Name ?? "";
        var contribution = factory?.Summary?.GetGroup(groupState.Definition).GetScaledContribution() ?? ItemVolumesState.Empty;
        return new SelectedObjectModel
        {
            Object = obj,
            SortedItemVolumes = groupState.ItemVolumes
                .Select(x => new FormattedItemVolume(x.Item.Identifier.Name, x.Volume, ItemVolumeColourScheme.Area, contribution.TryGetDetails(x.Item, out _)))
                .ToArray(),
            Description =
            {
                { "Group", groupState.GetPreferredName() },
                { "In region", regionName },
                { "Count", groupState.Definition.Repeat > 1 ? groupState.Definition.Repeat.ToString(CultureInfo.CurrentCulture) : "" },
            },
        };
    }

    private SelectedObjectModel CreateSelectedProduction(DrawingObject obj, GroupState groupState)
    {
        var region = graph?.GetNodeContainment(obj as Node)
            .Select(x => x?.UserData)
            .OfType<Maybe<Region>>()
            .Where(x => x.Exists)
            .Select(x => x.Value)
            .LastOrDefault();
        var regionName = region?.RegionName.Name ?? "";
        var contribution = factory?.Summary?.GetGroup(groupState.Definition).GetScaledContribution() ?? ItemVolumesState.Empty;
        return new SelectedObjectModel
        {
            Object = obj,
            SortedItemVolumes = groupState.ItemVolumes
                .Select(x => new FormattedItemVolume(x.Item.Identifier.Name, x.Volume, ItemVolumeColourScheme.Area, contribution.TryGetDetails(x.Item, out _)))
                .ToArray(),
            Description =
            {
                { "Group", groupState.GroupName?.Name },
                { "In region", regionName },
                { "Recipe", groupState.Production!.Recipe.RecipeName.Name },
                { "Count", FormatRational(groupState.Production!.Definition.Count) },
                { "Effective speed", FormatRational(groupState.Production!.Definition.EffectiveCount, "0.####%"), "Relative to a single factory at 100% clock speed." },
                { "Clocks", string.Join(", ", groupState.Production!.Definition.Clocks.Select(x => $"{x.Count} @ {FormatRational(x.Percentage, "0.####%")}")) },
            },
        };
    }

    private SelectedObjectModel CreateSelectedNetwork(DrawingObject obj, Identifier network)
    {
        var connections = factory?.State.TransportLinks.Where(x => x.NetworkName == network).ToArray() ?? Array.Empty<TransportLink>();
        var collectsFrom = connections.Where(x => x.Direction == TransportLinkDirection.FromRegion);
        var deliversTo = connections.Where(x => x.Direction == TransportLinkDirection.ToRegion);
        return new SelectedObjectModel
        {
            Object = obj,
            SortedItemVolumes = GetSortedItemVolumes(factory?.Summary?.GetNetwork(network))
                .Select(x => new FormattedItemVolume(x.Item.Identifier.Name, x.Volume, ItemVolumeColourScheme.Area, false))
                .ToArray(),
            Description =
            {
                { "Network", network.Name },
                { "Collects from", string.Join(", ", collectsFrom.Select(x => x.Region.RegionName.Name).Distinct()) },
                { "Delivers to", string.Join(", ", deliversTo.Select(x => x.Region.RegionName.Name).Distinct()) },
            },
        };
    }

    private SelectedObjectModel CreateSelectedTransportLink(DrawingObject obj, TransportLinkAggregate linkAggregate)
    {
        if (linkAggregate.Direction == TransportLinkDirection.ToRegion)
        {
            return new SelectedObjectModel
            {
                Object = obj,
                SortedItemVolumes = GetSortedItemVolumes(factory?.Summary?.GetTransportLink(linkAggregate))
                    .Select(x => new FormattedItemVolume(x.Item.Identifier.Name, x.Volume, ItemVolumeColourScheme.Transport, false))
                    .ToArray(),
                Description =
                {
                    { "From network", linkAggregate.NetworkName.Name },
                    { "To region", linkAggregate.Region.RegionName.Name },
                },
            };
        }
        if (linkAggregate.Direction == TransportLinkDirection.FromRegion)
        {
            return new SelectedObjectModel
            {
                Object = obj,
                SortedItemVolumes = GetSortedItemVolumes(factory?.Summary?.GetTransportLink(linkAggregate))
                    .Select(x => new FormattedItemVolume(x.Item.Identifier.Name, x.Volume, ItemVolumeColourScheme.Transport, false))
                    .ToArray(),
                Description =
                {
                    { "From region", linkAggregate.Region.RegionName.Name },
                    { "To network", linkAggregate.NetworkName.Name },
                },
            };
        }
        return SelectedObjectModel.None;
    }

    private IEnumerable<ItemVolume> GetSortedItemVolumes(IItemVolumesState? state)
    {
        return (state ?? ItemVolumesState.Empty)
            .Where(x => x.Volume != 0)
            // All positive first.
            .OrderByDescending(x => x.Volume.Sign)
            // Then alphabetical.
            .ThenBy(x => x.Item.Identifier.Name, StringComparer.OrdinalIgnoreCase);
    }

    private string FormatRational(Rational? rational, string? decimalFormatString = null)
    {
        // We almost never want to see fractions. Convert to decimal and format accordingly.
        if (rational == null) return "";
        if (decimalFormatString == null) return ((decimal)rational).ToString(CultureInfo.CurrentCulture);
        return ((decimal)rational).ToString(decimalFormatString, CultureInfo.CurrentCulture);
    }
}
