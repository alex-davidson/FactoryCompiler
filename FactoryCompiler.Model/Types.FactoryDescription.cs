using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Rationals;

namespace FactoryCompiler.Model;

public record FactoryDescription(ImmutableArray<Region> Regions)
{
    public virtual bool Equals(FactoryDescription? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Regions.SetEquals(other.Regions);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Regions.FirstOrDefault()?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) + Regions.Length.GetHashCode();
            return hashCode;
        }
    }

    public static FactoryDescription Merge(IEnumerable<FactoryDescription> factories) =>
        new FactoryDescription(Region.MergeByName(factories.SelectMany(x => x.Regions)));
}
public record Region(Identifier RegionName, ImmutableArray<Group> Groups, ImmutableArray<Transport> Inbound, ImmutableArray<Transport> Outbound)
{
    public virtual bool Equals(Region? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return RegionName.Equals(other.RegionName) &&
               Groups.SetEquals(other.Groups) &&
               Inbound.SetEquals(other.Inbound) &&
               Outbound.SetEquals(other.Outbound);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = RegionName.GetHashCode();
            hashCode = (hashCode * 397) + Groups.Length.GetHashCode();
            hashCode = (hashCode * 397) + Inbound.Length.GetHashCode();
            hashCode = (hashCode * 397) + Outbound.Length.GetHashCode();
            return hashCode;
        }
    }

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
}
public record Group
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

    public void Deconstruct(out Identifier? groupName, out Production? production, out ImmutableArray<Group> groups)
    {
        groupName = GroupName;
        production = Production;
        groups = Groups;
    }

    public virtual bool Equals(Group? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return GroupName.Equals(other.GroupName) &&
               Equals(Production, other.Production) &&
               Groups.SetEquals(other.Groups);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = GroupName?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) + (Production?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) + Groups.Length.GetHashCode();
            return hashCode;
        }
    }
}
public record Production(Identifier? FactoryName, Identifier RecipeName, Rational Count);
public record Transport(Identifier ItemName, Identifier Network);
