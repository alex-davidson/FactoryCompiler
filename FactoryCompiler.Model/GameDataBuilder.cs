using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FactoryCompiler.Model;

public class GameDataBuilder
{
    public static IGameData GetDefaultGameData() => new DefaultGameData().Build();

    public ICollection<Factory> Factories { get; } = new List<Factory>();
    public ICollection<Recipe> Recipes { get; } = new List<Recipe>();

    public IGameData Build() => new GameData(this);

    private class GameData : IGameData
    {
        internal GameData(GameDataBuilder builder)
        {
            Factories = builder.Factories.ToImmutableHashSet();
            Recipes = builder.Recipes.ToImmutableHashSet();
            AllItems = Union(
                Recipes.SelectMany(r => r.Inputs).Select(r => r.Item),
                Recipes.SelectMany(r => r.Outputs).Select(r => r.Item));
            BaseItems = Recipes.SelectMany(r => r.Inputs).Select(r => r.Item)
                .Except(Recipes.SelectMany(r => r.Outputs).Select(r => r.Item))
                .ToImmutableHashSet();
        }

        private static ImmutableHashSet<Item> Union(params IEnumerable<Item>[] sources)
        {
            var set = ImmutableHashSet.CreateBuilder<Item>();
            foreach (var source in sources) set.UnionWith(source);
            return set.ToImmutable();
        }

        public ImmutableHashSet<Item> BaseItems { get; init; }

        /// <summary>
        /// All known items.
        /// </summary>
        public ImmutableHashSet<Item> AllItems { get; init; }

        public ImmutableHashSet<Recipe> Recipes { get; init; }
        public ImmutableHashSet<Factory> Factories { get; init; }
    }
}
