using System.Collections.Generic;
using System.Linq;
using Microsoft.Msagl.Drawing;

namespace FactoryCompiler.Jobs.Visualise;

public static class GraphExtensions
{
    /// <summary>
    /// Returns the node's containing subgraphs, closest first.
    /// </summary>
    public static IEnumerable<Node> GetNodeContainment(this Graph graph, Node? node)
    {
        if (node == null) yield break;
        var subgraph = graph.SubgraphMap.Values.FirstOrDefault(x => x.Nodes.Contains(node));
        while (subgraph != null)
        {
            yield return subgraph;
            subgraph = subgraph.ParentSubgraph;
        }
    }
}
