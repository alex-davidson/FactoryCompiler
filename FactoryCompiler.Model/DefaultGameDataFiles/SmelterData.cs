using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class SmelterData : DefaultGameDataBase, IFactoryData
{
    public static Factory Smelter { get; } = new Factory("Smelter");

    public Factory Factory => Smelter;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Caterium Ingot", new Duration(4),
            Smelter,
            Item.List(new Item("Caterium Ore").Volume(3)),
            Item.List(new Item("Caterium Ingot").Volume(1))),
        new Recipe("Copper Ingot", new Duration(2),
            Smelter,
            Item.List(new Item("Copper Ore").Volume(1)),
            Item.List(new Item("Copper Ingot").Volume(1))),

        new Recipe("Iron Ingot", new Duration(2),
            Smelter,
            Item.List(new Item("Iron Ore").Volume(1)),
            Item.List(new Item("Iron Ingot").Volume(1))),

        new Recipe("Pure Aluminum Ingot", new Duration(2),
            Smelter,
            Item.List(new Item("Aluminum Scrap").Volume(2)),
            Item.List(new Item("Aluminum Ingot").Volume(1))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
