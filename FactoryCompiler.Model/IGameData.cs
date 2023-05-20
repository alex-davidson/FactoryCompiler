using System.Collections.Immutable;

namespace FactoryCompiler.Model;

public interface IGameData
{
    /// <summary>
    /// Natural resources.
    /// </summary>
    ImmutableHashSet<Item> BaseItems { get; }

    /// <summary>
    /// All known items.
    /// </summary>
    ImmutableHashSet<Item> AllItems { get; }

    ImmutableHashSet<Factory> Factories { get; }
    ImmutableHashSet<Recipe> Recipes { get; }
}
