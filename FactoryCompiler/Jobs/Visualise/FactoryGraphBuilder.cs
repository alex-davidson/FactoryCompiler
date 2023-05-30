using System;
using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.State;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Node = Microsoft.Msagl.Drawing.Node;

namespace FactoryCompiler.Jobs.Visualise;

internal class FactoryGraphBuilder
{
    public Graph Build(FactoryState factoryState)
    {
        var builder = new GraphBuilder();
        builder.Graph.Directed = true;
        builder.Graph.LayoutAlgorithmSettings = new SugiyamaLayoutSettings();
        foreach (var region in factoryState.Regions)
        {
            BuildRegion(builder, region);
        }

        foreach (var network in factoryState.TransportLinks.GroupBy(x => x.NetworkName))
        {
            BuildNetworkWithTransportLinks(builder, network);
        }
        return builder.Graph;
    }

    private static LayoutAlgorithmSettings CreateRegionLayout() => new SugiyamaLayoutSettings { LabelCornersPreserveCoefficient = 1 };
    private static LayoutAlgorithmSettings CreateGroupLayout() => new SugiyamaLayoutSettings { LabelCornersPreserveCoefficient = 1 };

    private static void BuildRegion(GraphBuilder builder, RegionState region)
    {
        var regionSubgraph = builder.CreateSubgraph<Region>(region.Definition, region.RegionName.Name);
        builder.Graph.RootSubgraph.AddSubgraph(regionSubgraph);

        var layout = CreateRegionLayout();
        regionSubgraph.LayoutSettings = layout;
        layout.ClusterMargin = 30;
        (builder.Graph.LayoutAlgorithmSettings as SugiyamaLayoutSettings)?.ClusterSettings.Add(regionSubgraph, layout);

        foreach (var group in region.Groups)
        {
            BuildGroup(builder, regionSubgraph, group);
        }
    }

    private static Node BuildGroup(GraphBuilder builder, Subgraph parent, GroupState group)
    {
        if (group.Production != null)
        {
            return BuildProduction(builder, parent, group);
        }

        if (!group.Groups.Any(x => x.Definition.Visible))
        {
            var groupNode = builder.CreateNode<GroupState>(group, group.GroupName?.Name);
            builder.Graph.AddNode(groupNode);
            parent.AddNode(groupNode);
            return groupNode;
        }

        var groupSubgraph = builder.CreateSubgraph<GroupState>(group, group.GroupName?.Name);
        parent.AddSubgraph(groupSubgraph);

        var layout = CreateGroupLayout();
        layout.PackingMethod = PackingMethod.Columns;

        groupSubgraph.LayoutSettings = layout;
        (builder.Graph.LayoutAlgorithmSettings as SugiyamaLayoutSettings)?.ClusterSettings.Add(groupSubgraph, layout);

        foreach (var childGroup in group.Groups)
        {
            if (!childGroup.Definition.Visible) continue;
            BuildGroup(builder, groupSubgraph, childGroup);
        }
        return groupSubgraph;
    }

    private static Node BuildProduction(GraphBuilder builder, Subgraph parent, GroupState group)
    {
        if (group.Production == null) throw new ArgumentException("Must be a production group.");
        var productionNode = builder.CreateNode<GroupState>(group, group.GetPreferredName());
        builder.Graph.AddNode(productionNode);
        parent.AddNode(productionNode);
        return productionNode;
    }

    private static void BuildNetworkWithTransportLinks(GraphBuilder builder, IGrouping<Identifier, TransportLink> network)
    {
        var networkNode = builder.CreateNode<Identifier>(network.Key, network.Key.Name);
        builder.Graph.AddNode(networkNode);
        networkNode.Attr.FillColor = Color.LightGray;
        foreach (var link in network.Select(x => new { x.Direction, x.Region }).Distinct())
        {
            var regionSubgraph = builder.Find<Region>(link.Region);
            if (regionSubgraph == null) continue;
            var linkAggregate = new TransportLinkAggregate(link.Direction, link.Region, network.Key);
            if (link.Direction == TransportLinkDirection.FromRegion)
            {
                var edge = builder.Graph.AddEdge(regionSubgraph.Id, networkNode.Id);
                edge.UserData = (Maybe<TransportLinkAggregate>)linkAggregate;
                edge.LabelText = "";
            }
            else if (link.Direction == TransportLinkDirection.ToRegion)
            {
                var edge = builder.Graph.AddEdge(networkNode.Id, regionSubgraph.Id);
                edge.UserData = (Maybe<TransportLinkAggregate>)linkAggregate;
                edge.LabelText = "";
            }
        }
    }
}
