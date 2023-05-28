using System.Collections.Generic;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Node = Microsoft.Msagl.Drawing.Node;

namespace FactoryCompiler.Jobs.Visualise;

internal class GraphBuilder
{
    private int nextId = 1;

    public Graph Graph { get; } = new Graph();

    private readonly Dictionary<object, Node> nodeIndex = new Dictionary<object, Node>();

    public Node? Find<T>(Maybe<T> obj)
    {
        if (obj.Exists)
        {
            if (nodeIndex.TryGetValue(obj.Value, out var node))
            {
                return node;
            }
        }
        return null;
    }

    public Subgraph CreateSubgraph<T>(Maybe<T> obj, string? name)
    {
        var subgraph = new Subgraph(NextId(obj, name)) { LabelText = name ?? "", UserData = obj };
        subgraph.Attr.ClusterLabelMargin = LabelPlacement.Top;
        subgraph.DiameterOfOpenCollapseButton = 20;

        if (obj.Exists) nodeIndex[obj.Value] = subgraph;
        return subgraph;
    }

    public Node CreateNode<T>(Maybe<T> obj, string? name)
    {
        var node = new Node(NextId(obj, name)) { LabelText = name ?? "", UserData = obj };
        if (obj.Exists) nodeIndex[obj.Value] = node;
        return node;
    }

    private string NextId<T>(Maybe<T> obj, string? name)
    {
        var id = nextId;
        nextId++;
        return string.Concat(obj.Type.Name, " _", name ?? "Anon", "_", id.ToString());
    }
}
