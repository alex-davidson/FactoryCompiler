using System.Collections.Generic;
using System.Linq;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal static class MiningDataHelper
{
    private static readonly Item[] oreTypes =
    {
        new Item("Bauxite"),
        new Item("Caterium Ore"),
        new Item("Coal"),
        new Item("Copper Ore"),
        new Item("Iron Ore"),
        new Item("Limestone"),
        new Item("Raw Quartz"),
        new Item("SAM Ore"),
        new Item("Sulfur"),
        new Item("Uranium"),
    };

    private static IEnumerable<Recipe> Mining(Factory miner, Item ore, int multiplier)
    {
        yield return new Recipe($"Impure {ore.Identifier.Name}", new Duration(60),
            miner,
            Item.NoItems,
            Item.List(ore.Volume(30 * multiplier)));
        yield return new Recipe($"Normal {ore.Identifier.Name}", new Duration(60),
            miner,
            Item.NoItems,
            Item.List(ore.Volume(60 * multiplier)));
        yield return new Recipe($"Pure {ore.Identifier.Name}", new Duration(60),
            miner,
            Item.NoItems,
            Item.List(ore.Volume(120 * multiplier)));
    }

    public static Recipe[] GetMiningRecipes(Factory miner, int multiplier) =>
        oreTypes.SelectMany(o => Mining(miner, o, multiplier)).ToArray();
}
