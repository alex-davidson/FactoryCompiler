using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Rationals;

namespace FactoryCompiler.Model;

public class FactoryDescription
{
    public FactoryDescription(ImmutableArray<Region> regions)
    {
        Regions = regions;
    }

    public ImmutableArray<Region> Regions { get; init; }

    public static FactoryDescription Merge(IEnumerable<FactoryDescription> factories) =>
        new FactoryDescription(Region.MergeByName(factories.SelectMany(x => x.Regions)));

    public readonly struct EquivalenceComparer : IEqualityComparer<FactoryDescription>
    {
        public bool Equals(FactoryDescription? x, FactoryDescription? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Regions.SetEquals(y.Regions, default(Region.EquivalenceComparer));
        }

        public int GetHashCode(FactoryDescription obj)
        {
            return HashCode.Combine(obj.Regions.FirstOrDefault(), obj.Regions.Length);
        }
    }
}

public class Region
{
    public Region(Identifier regionName, ImmutableArray<Group> groups, ImmutableArray<Transport> inbound, ImmutableArray<Transport> outbound)
    {
        RegionName = regionName;
        Groups = groups;
        Inbound = inbound;
        Outbound = outbound;
    }

    public Identifier RegionName { get; init; }
    public ImmutableArray<Group> Groups { get; init; }
    public ImmutableArray<Transport> Inbound { get; init; }
    public ImmutableArray<Transport> Outbound { get; init; }

    public static ImmutableArray<Region> MergeByName(IEnumerable<Region> regions)
    {
        return regions.GroupBy(x => x.RegionName)
            .Select(xs => new Region(
                xs.Key,
                xs.SelectMany(x => x.Groups).ToImmutableArray(),
                xs.SelectMany(x => x.Inbound).Distinct().ToImmutableArray(),
                xs.SelectMany(x => x.Outbound).Distinct().ToImmutableArray()))
            .ToImmutableArray();
    }

    public readonly struct EquivalenceComparer : IEqualityComparer<Region>
    {
        public bool Equals(Region? x, Region? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.RegionName.Equals(y.RegionName) &&
                   x.Groups.SetEquals(y.Groups, default(Group.EquivalenceComparer)) &&
                   x.Inbound.SetEquals(y.Inbound) &&
                   x.Outbound.SetEquals(y.Outbound);
        }

        public int GetHashCode(Region obj)
        {
            return HashCode.Combine(obj.RegionName, obj.Groups.Length, obj.Inbound.Length, obj.Outbound.Length);
        }
    }
}

public class Group
{
    public Group(Identifier? groupName, Production? production, int repeat = 1)
    {
        GroupName = groupName;
        Production = production;
        Repeat = repeat;
        Groups = ImmutableArray<Group>.Empty;
    }

    public Group(Identifier? groupName, ImmutableArray<Group> groups, int repeat = 1)
    {
        GroupName = groupName;
        Groups = groups;
        Repeat = repeat;
    }

    public Identifier? GroupName { get; }
    public Production? Production { get; }
    public ImmutableArray<Group> Groups { get; }
    public int Repeat { get; }

    public readonly struct EquivalenceComparer : IEqualityComparer<Group>
    {
        public bool Equals(Group? x, Group? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            var areChildrenEquiv = x.Groups.SetEquals(y.Groups, default(EquivalenceComparer));

            return x.GroupName.Equals(y.GroupName) &&
                   default(Production.EquivalenceComparer).Equals(x.Production, y.Production) &&
                   areChildrenEquiv;
        }

        public int GetHashCode(Group obj)
        {
            return HashCode.Combine(obj.GroupName,
                obj.Production == null ? 0 : default(Production.EquivalenceComparer).GetHashCode(obj.Production),
                obj.Groups.Length);
        }
    }
}

public class Production
{
    public Production(Identifier? factoryName, Identifier recipeName, Rational count)
    {
        FactoryName = factoryName;
        RecipeName = recipeName;
        Count = count;
    }

    public Identifier? FactoryName { get; init; }
    public Identifier RecipeName { get; init; }
    public Rational Count { get; init; }

    public readonly struct EquivalenceComparer : IEqualityComparer<Production>
    {
        public bool Equals(Production? x, Production? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return Equals(x.FactoryName, y.FactoryName) &&
                   Equals(x.RecipeName, y.RecipeName) &&
                   Equals(x.Count, y.Count);
        }

        public int GetHashCode(Production obj)
        {
            return HashCode.Combine(obj.FactoryName, obj.RecipeName);
        }
    }
}

public record Transport(Identifier ItemName, Identifier Network);
