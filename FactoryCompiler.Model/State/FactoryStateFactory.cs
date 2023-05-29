using FactoryCompiler.Model.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FactoryCompiler.Model.State
{
    /// <summary>
    /// Converts a FactoryDescription into a mutable FactoryState, optionally simplifying
    /// the structure as it goes.
    /// </summary>
    /// <remarks>
    /// FactoryState retains links to its original FactoryDescription, but adds resolved
    /// information from GameData (ie. specific recipes) and can track item production.
    /// Validation is also performed by this class: failures will be included on the
    /// FactoryState and invalid nodes will be omitted.
    /// </remarks>
    public class FactoryStateFactory
    {
        private readonly ILookup<Identifier, Recipe> recipes;
        private readonly Dictionary<Identifier, Item> items;

        /// <summary>
        /// If a group contains only one subgroup, merge them.
        /// </summary>
        public bool CollapseGroups { get; set; }

        public FactoryStateFactory(IGameData gameData)
        {
            recipes = gameData.Recipes.ToLookup(x => x.RecipeName);
            items = gameData.AllItems.ToDictionary(x => x.Identifier);
        }

        public FactoryState Create(FactoryDescription description)
        {
            var builder = new ValidatingBuilder(recipes, items)
            {
                CollapseGroups = CollapseGroups,
            };
            var regions = description.Regions.Select(builder.BuildRegion).ToImmutableArray();
            var transportLinks = description.Regions.SelectMany(builder.BuildTransportLinks).ToImmutableArray();

            return new FactoryState(regions, transportLinks, builder.Diagnostics.ToImmutableArray());
        }

        private class ValidatingBuilder
        {
            private readonly ILookup<Identifier, Recipe> recipes;
            private readonly Dictionary<Identifier, Item> items;

            public ValidatingBuilder(ILookup<Identifier, Recipe> recipes, Dictionary<Identifier, Item> items)
            {
                this.recipes = recipes;
                this.items = items;
            }

            public ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();
            public bool CollapseGroups { get; set; }

            public RegionState BuildRegion(Region region)
            {
                var groups = region.Groups.Select(x => BuildGroup(x, 1)).ToImmutableArray();
                return new RegionState(region, groups);
            }

            private GroupState BuildGroup(Group group, int parentRepeats)
            {
                if (group.Groups.Length == 0) return BuildProductionGroup(group, parentRepeats);

                var subgroups = group.Groups.Select(x => BuildGroup(x, parentRepeats * group.Repeat)).ToImmutableArray();
                Debug.Assert(subgroups.Length > 0, "Non-production Group should have subgroups.");
                if (CollapseGroups && subgroups.Length == 1)
                {
                    var subgroup = subgroups.Single();
                    if (group.GroupName == null) return subgroup;
                    if (subgroup.GroupName == null) return subgroup.WithGroupName(group.GroupName.Value);
                    // Both have names. Combine them.
                    return subgroup.WithGroupName(group.GroupName.Value.Name + ", " + subgroup.GroupName.Value.Name);
                }
                return new GroupState(group, subgroups, parentRepeats);
            }

            private GroupState BuildProductionGroup(Group group, int parentRepeats)
            {
                if (group.Production == null) throw new ArgumentException("Group is not a production group.");

                if (!TryResolveRecipe(group.Production.RecipeName, group.Production.FactoryName, out var recipe))
                {
                    return new GroupState(group, ImmutableArray<GroupState>.Empty, parentRepeats);
                }
                return new GroupState(group, new ProductionState(group.Production, recipe), parentRepeats);
            }

            private bool TryResolveRecipe(Identifier recipeName, Identifier? factoryName, [MaybeNullWhen(returnValue: false)] out Recipe recipe)
            {
                var matchesByName = recipes[recipeName].ToArray();
                if (matchesByName.Length == 0)
                {
                    Diagnostics.Add(Diagnostic.Warning($"Unrecognised recipe name: {recipeName}"));
                    recipe = null;
                    return false;
                }
                if (factoryName == null)
                {
                    recipe = matchesByName.First();
                    if (matchesByName.Length > 1)
                    {
                        Diagnostics.Add(Diagnostic.Warning(
                            $"Recipe '{recipeName}' is ambiguous and no factory name was specified. Assuming {recipe.MadeByFactory.FactoryName}."));
                    }
                    return true;
                }

                var matchesByFactoryName = matchesByName.Where(x => x.MadeByFactory.FactoryName == factoryName).ToArray();
                recipe = matchesByFactoryName.First();
                if (matchesByFactoryName.Length > 1)
                {
                    // ??1
                    Diagnostics.Add(Diagnostic.Warning(
                        $"Recipe '{recipeName}' is ambiguous for factory '{factoryName}'. Using first match."));
                }
                if (matchesByFactoryName.Length == 0)
                {
                    Diagnostics.Add(Diagnostic.Warning($"Recipe '{recipeName}' is not applicable to factory '{factoryName}'."));
                    return false;
                }
                return true;
            }

            public IEnumerable<TransportLink> BuildTransportLinks(Region region)
            {
                var conflicts = region.Inbound.Select(x => x.ItemName).
                    Intersect(region.Outbound.Select(x => x.ItemName))
                    .ToArray();
                foreach (var conflict in conflicts)
                {
                    Diagnostics.Add(Diagnostic.Error($"Region '{region.RegionName}' both imports and exports '{conflict}'."));
                }

                return BuildTransportLinks(TransportLinkDirection.ToRegion, region, region.Inbound)
                    .Concat(BuildTransportLinks(TransportLinkDirection.FromRegion, region, region.Outbound))
                    .Distinct();
            }

            private IEnumerable<TransportLink> BuildTransportLinks(TransportLinkDirection direction, Region region, IEnumerable<Transport> transports)
            {
                foreach (var transport in transports)
                {
                    if (!items.TryGetValue(transport.ItemName, out var item))
                    {
                        Diagnostics.Add(Diagnostic.Warning($"Unrecognised item name: {transport.ItemName}"));
                        continue;
                    }
                    yield return new TransportLink(direction, region, transport.Network, item);
                }
            }
        }
    }
}
