using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FactoryCompiler.Model.State;
using Rationals;
using Shared.LinearSolver;
using Shared.LinearSolver.Constraints;

namespace FactoryCompiler.Model.Algorithms
{
    public class TransportGraphAnalyser
    {
        public IReadOnlyCollection<ImmutableArray<Edge>> GetCycles(IEnumerable<TransportLink> subgraph)
        {
            // A proper graph-handling library could probably do this better...

            // * There are no direct links between network nodes, or between region nodes. Therefore
            //   the cycle size can only ever be an even number.
            // * The subgraph can be redrawn in terms of just regions, and any cycles will still exist.

            // Depth-first traversal of simplified graph:
            var edgesBySourceRegion = RewriteWithoutNetworks(subgraph).ToLookup(x => x.From);

            var cycles = new List<ImmutableArray<Edge>>();
            var visited = new HashSet<Region>();
            var stack = new Stack<Edge>();
            foreach (var region in edgesBySourceRegion.Select(x => x.Key))
            {
                ExploreFrom(region);
            }
            return cycles;

            void ExploreFrom(Region current)
            {
                if (!visited.Add(current)) return; // Already explored.
                foreach (var edge in edgesBySourceRegion[current])
                {
                    if (stack.Any(x => x.From == edge.To))
                    {
                        cycles.Add(ImmutableArray.CreateRange(stack.Append(edge)));
                        continue;
                    }
                    stack.Push(edge);
                    ExploreFrom(edge.To);
                }
            }
        }

        private IEnumerable<Edge> RewriteWithoutNetworks(IEnumerable<TransportLink> links)
        {
            var directed = links.ToLookup(x => x.Direction);
            // Cartesian product across each network node.
            return directed[TransportLinkDirection.FromRegion].Join(
                directed[TransportLinkDirection.ToRegion],
                f => f.NetworkName, t => t.NetworkName,
                (f, t) => new Edge(f.Region, t.Region, f.NetworkName));
        }

        public readonly struct Edge
        {
            public Edge(Region from, Region to, Identifier via)
            {
                From = from;
                To = to;
                Via = via;
            }

            public Region From { get; }
            public Region To { get; }
            public Identifier Via { get; }
        }

        private readonly bool debugToConsole = false;

        /// <summary>
        /// Use Simplex method to determine resource distribution.
        /// </summary>
        /// <param name="links">Transport links involved.</param>
        /// <param name="itemType">Item type for which we're currently solving. Caller is expected to filter/group the links appropriately.</param>
        /// <param name="tradeLimits">Supply/demand volumes for regions involved.</param>
        public IReadOnlyCollection<Distribution> SolveDistribution(IReadOnlyCollection<TransportLink> links, Item itemType, Dictionary<Region, IItemVolumesState> tradeLimits)
        {
            // Optimisation problem, solvable using Simplex.

            // * One unknown per link.
            // * For each source: constrain sum of outgoing links + surplus == source supply, ie. don't demand shortfall, and record any excess.
            // * For each sink: constrain sum of incoming links <= sink demand, ie. don't supply excess.
            // * For each network: constrain sum of incoming links minus sum of outgoing links to be <= 0, ie. networks never demand excess.
            // * Maximise sum of all links to sinks.
            // Note that Simplex requires that all solutions be positive.
            var constraints = new ConstraintList();

            // For use when debugging test fixtures, etc. Shows Simplex tableau as well as inputs/outputs.
            var debugWriter = debugToConsole ? new DebugWriter(Console.WriteLine) : null;

            // Link's index is its variable number.
            var linkVariables = links.Select((x, i) => new Variable(x, i)).ToArray();
            // Simplex solver usually adds surplus variables internally for inequalities, but in this case
            // we actually want them as outputs.
            var surplusVariables = linkVariables
                .Where(x => x.Link.Direction == TransportLinkDirection.FromRegion)
                .GroupBy(x => x.Link.Region)
                .Select((x, i) => new RegionSurplusVariable(x.Key, linkVariables.Length + i, x.Count()))
                .ToDictionary(x => x.Region);
            var totalVariables = linkVariables.Length + surplusVariables.Count;

            var maximise = new float[totalVariables];
            foreach (var source in linkVariables.Where(x => x.Link.Direction == TransportLinkDirection.FromRegion).GroupBy(x => x.Link.Region))
            {
                var supply = tradeLimits.TryGetValue(source.Key, out var volumes) ? volumes.GetNetVolume(itemType) : 0;
                if (supply <= 0) supply = 0;    // Nothing to export.
                var coefficients = new float[totalVariables];
                foreach (var variable in source)
                {
                    coefficients[variable.Index] = 1;
                    maximise[variable.Index] = 1;
                }
                if (surplusVariables.TryGetValue(source.Key, out var surplusVariable))
                {
                    coefficients[surplusVariable.Index] = 1;
                }
                debugWriter?.Write($"[{string.Join(", ", coefficients)}] <= {supply}");
                constraints.Add(Constrain.Linear(coefficients).EqualTo((float)supply));
            }
            foreach (var sink in linkVariables.Where(x => x.Link.Direction == TransportLinkDirection.ToRegion).GroupBy(x => x.Link.Region))
            {
                var demand = tradeLimits.TryGetValue(sink.Key, out var volumes) ? volumes.GetNetVolume(itemType) : 0;
                if (demand >= 0) demand = 0;    // Nothing to import.
                var coefficients = new float[totalVariables];
                foreach (var variable in sink)
                {
                    coefficients[variable.Index] = 1;
                    maximise[variable.Index] = 1;
                }
                debugWriter?.Write($"[{string.Join(", ", coefficients)}] <= {demand.AbsoluteValue()}");
                constraints.Add(Constrain.Linear(coefficients).LessThanOrEqualTo((float)demand.AbsoluteValue()));
            }
            foreach (var network in linkVariables.GroupBy(x => x.Link.NetworkName))
            {
                var coefficients = new float[totalVariables];
                foreach (var variable in network)
                {
                    coefficients[variable.Index] = variable.Link.Direction == TransportLinkDirection.FromRegion ? 1
                                                 : variable.Link.Direction == TransportLinkDirection.ToRegion ? -1
                                                 : 0;
                }
                debugWriter?.Write($"[{string.Join(", ", coefficients)}] <= 0");
                constraints.Add(Constrain.Linear(coefficients).LessThanOrEqualTo(0));
            }

            debugWriter?.Write($"[{string.Join(", ", maximise)}]  max");
            var result = SimplexSolver.Given(constraints, debugWriter).Maximise(maximise);
            switch (result.Result)
            {
                case SimplexResult.OptimalSolution:
                case SimplexResult.OptimalSolutionNotUnique:
                    break;

                default:
                    throw new Exception($"Solver failed: {result.Result}");
            }

            foreach (var line in linkVariables)
            {
                var r = ReadResult(line);
                debugWriter?.Write($"x{line.Index + 1}> {line.Link.NetworkName} {line.Link.Direction} {line.Link.Region.RegionName}: {r.ItemVolume.Volume}");
            }

            var distributions = linkVariables.Select(ReadResult).ToArray();
            // We probably still have surplus resources from each region. Account for these as surplus in networks/links by evenly distributing it.
            return distributions;

            Distribution ReadResult(Variable variable)
            {
                var value = Rational.Approximate(result.Values[variable.Index]);
                if (variable.Link.Direction == TransportLinkDirection.FromRegion)
                {
                    // Take our share of any excess in that region too.
                    var surplus = surplusVariables.TryGetValue(variable.Link.Region, out var surplusVariable) ? ReadValue(surplusVariable.Index) / surplusVariable.DestinationCount : 0;
                    return new Distribution(variable.Link, itemType.Volume(value + surplus));
                }
                if (variable.Link.Direction == TransportLinkDirection.ToRegion) return new Distribution(variable.Link, itemType.Volume(-value));
                return new Distribution(variable.Link, itemType.Volume(0));
            }

            Rational ReadValue(int index) => Rational.Approximate(result.Values[index]);
        }

        private readonly struct Variable
        {
            public Variable(TransportLink link, int index)
            {
                Link = link;
                Index = index;
            }

            public int Index { get; }
            public TransportLink Link { get; }
        }

        private readonly struct RegionSurplusVariable
        {
            public RegionSurplusVariable(Region region, int index, int destinationCount)
            {
                Region = region;
                Index = index;
                DestinationCount = destinationCount;
            }

            public Region Region { get; }
            public int Index { get; }
            public int DestinationCount { get; }
        }

        public readonly struct Distribution
        {
            public Distribution(TransportLink link, ItemVolume itemVolume)
            {
                Link = link;
                ItemVolume = itemVolume;
            }

            public TransportLink Link { get; }
            public ItemVolume ItemVolume { get; }
        }
    }
}
