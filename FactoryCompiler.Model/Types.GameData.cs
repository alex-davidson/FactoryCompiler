using System;
using System.Collections.Immutable;
using Rationals;

/// <summary>
/// Conventions:
/// * Durations are always given as a rational number of seconds.
/// * Percentages are always given as a rational number, eg. 50% is 1/2.
/// </summary>
namespace FactoryCompiler.Model;

public readonly record struct Identifier(string Name)
{
    public static bool TryCreate(string name, out Identifier identifier)
    {
        identifier = new Identifier(name);
        return identifier.IsValid;
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(Name);
    public static implicit operator Identifier(string name) => new Identifier(name);
    public override string ToString() => Name;

    public static ImmutableArray<Identifier> EmptyList => ImmutableArray<Identifier>.Empty;
    public static ImmutableArray<Identifier> List(params Identifier[] identifiers) => ImmutableArray.CreateRange(identifiers);
    public static implicit operator ImmutableArray<Identifier>(Identifier single) => ImmutableArray.Create(single);
}
public record Item(Identifier Identifier)
{
    public static ImmutableArray<ItemVolume> NoItems => ImmutableArray<ItemVolume>.Empty;
    public static ImmutableArray<Item> List(params Item[] identifiers) => ImmutableArray.CreateRange(identifiers);
    public static ImmutableArray<ItemVolume> List(params ItemVolume[] identifiers) => ImmutableArray.CreateRange(identifiers);
}
public record ItemVolume(Item Item, Rational Volume);
public record Factory(Identifier FactoryName);

[Flags]
public enum RecipeFlags
{
    None = 0,
    Alternate = 0b0001,
    ProjectAssembly = 0b0010,
    FICSMAS = 0b0100,
}
public record Recipe(Identifier RecipeName, Duration BaseDuration, Factory MadeByFactory, ImmutableArray<ItemVolume> Inputs, ImmutableArray<ItemVolume> Outputs, RecipeFlags Flags = RecipeFlags.None)
{
    public virtual bool Equals(Recipe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return RecipeName.Equals(other.RecipeName) &&
               BaseDuration.Equals(other.BaseDuration) &&
               MadeByFactory.Equals(other.MadeByFactory) &&
               Inputs.SetEquals(other.Inputs) &&
               Outputs.SetEquals(other.Outputs) &&
               Flags.Equals(other.Flags);
    }

    public override int GetHashCode() => RecipeName.GetHashCode();
}

public readonly record struct Percentage(decimal Percent)
{
    public static implicit operator Rational(Percentage percentage) => (Rational)percentage.Percent / 100;
}

public readonly record struct Duration(decimal Seconds)
{
    public static implicit operator Rational(Duration duration) => (Rational)duration.Seconds;
}

public static class Extensions
{
    public static ItemVolume Volume(this Item item, Rational volume) => new ItemVolume(item, volume);
}
