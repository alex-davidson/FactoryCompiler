using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class FoundryData : DefaultGameDataBase, IFactoryData
{
    public static Factory Foundry { get; } = new Factory("Foundry");

    public Factory Factory => Foundry;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Aluminum Ingot", new Duration(4),
            Foundry,
            Item.List(
                new Item("Aluminum Scrap").Volume(6),
                new Item("Silica").Volume(5)),
            Item.List(new Item("Aluminum Ingot").Volume(4))),

        new Recipe("Steel Ingot", new Duration(4),
            Foundry,
            Item.List(
                new Item("Iron Ore").Volume(3),
                new Item("Coal").Volume(3)),
            Item.List(new Item("Steel Ingot").Volume(3))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
        new Recipe("Coke Steel Ingot", new Duration(12),
            Foundry,
            Item.List(
                new Item("Iron Ore").Volume(15),
                new Item("Petroleum Coke").Volume(15)),
            Item.List(new Item("Steel Ingot").Volume(20)),
            RecipeFlags.Alternate),
        new Recipe("Compacted Steel Ingot", new Duration(16),
            Foundry,
            Item.List(
                new Item("Iron Ore").Volume(6),
                new Item("Compacted Coal").Volume(3)),
            Item.List(new Item("Steel Ingot").Volume(10)),
            RecipeFlags.Alternate),
        new Recipe("Copper Alloy Ingot", new Duration(12),
            Foundry,
            Item.List(
                new Item("Copper Ore").Volume(10),
                new Item("Iron Ore").Volume(5)),
            Item.List(new Item("Copper Ingot").Volume(20)),
            RecipeFlags.Alternate),

        new Recipe("Iron Alloy Ingot", new Duration(6),
            Foundry,
            Item.List(
                new Item("Iron Ore").Volume(2),
                new Item("Copper Ore").Volume(2)),
            Item.List(new Item("Iron Ingot").Volume(5)),
            RecipeFlags.Alternate),

        new Recipe("Solid Steel Ingot", new Duration(3),
            Foundry,
            Item.List(
                new Item("Iron Ingot").Volume(2),
                new Item("Coal").Volume(2)),
            Item.List(new Item("Steel Ingot").Volume(3)),
            RecipeFlags.Alternate),
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
