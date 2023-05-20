using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class RefineryData : DefaultGameDataBase, IFactoryData
{
    public static Factory Refinery { get; } = new Factory("Refinery");

    public Factory Factory => Refinery;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Alumina Solution", new Duration(6),
            Refinery,
            Item.List(
                new Item("Bauxite").Volume(12),
                new Item("Water").Volume(18)),
            Item.List(
                new Item("Alumina Solution").Volume(12),
                new Item("Silica").Volume(5))),
        new Recipe("Aluminum Scrap", new Duration(1),
            Refinery,
            Item.List(
                new Item("Alumina Solution").Volume(4),
                new Item("Coal").Volume(2)),
            Item.List(
                new Item("Aluminum Scrap").Volume(6),
                new Item("Water").Volume(2))),

        new Recipe("Fuel", new Duration(6),
            Refinery,
            Item.List(new Item("Crude Oil").Volume(6)),
            Item.List(
                new Item("Fuel").Volume(4),
                new Item("Polymer Resin").Volume(3))),

        new Recipe("Liquid Biofuel", new Duration(4),
            Refinery,
            Item.List(
                new Item("Solid Biofuel").Volume(6),
                new Item("Water").Volume(3)),
            Item.List(new Item("Liquid Biofuel").Volume(4))),

        new Recipe("Petroleum Coke", new Duration(6),
            Refinery,
            Item.List(new Item("Heavy Oil Residue").Volume(4)),
            Item.List(new Item("Petroleum Coke").Volume(12))),

        new Recipe("Plastic", new Duration(6),
            Refinery,
            Item.List(new Item("Crude Oil").Volume(3)),
            Item.List(
                new Item("Plastic").Volume(2),
                new Item("Heavy Oil Residue").Volume(1))),

        new Recipe("Polyester Fabric", new Duration(2),
            Refinery,
            Item.List(
                new Item("Polymer Resin").Volume(1),
                new Item("Water").Volume(1)),
            Item.List(new Item("Fabric").Volume(1))),

        new Recipe("Residual Fuel", new Duration(6),
            Refinery,
            Item.List(new Item("Heavy Oil Residue").Volume(6)),
            Item.List(new Item("Fuel").Volume(4))),
        new Recipe("Residual Plastic", new Duration(6),
            Refinery,
            Item.List(
                new Item("Heavy Oil Residue").Volume(6),
                new Item("Water").Volume(2)),
            Item.List(new Item("Plastic").Volume(2))),
        new Recipe("Residual Rubber", new Duration(6),
            Refinery,
            Item.List(
                new Item("Polymer Resin").Volume(4),
                new Item("Water").Volume(4)),
            Item.List(new Item("Rubber").Volume(2))),
        new Recipe("Rubber", new Duration(6),
            Refinery,
            Item.List(new Item("Crude Oil").Volume(3)),
            Item.List(
                new Item("Rubber").Volume(2),
                new Item("Heavy Oil Residue").Volume(2))),

        new Recipe("Smokeless Powder", new Duration(6),
            Refinery,
            Item.List(
                new Item("Black Powder").Volume(2),
                new Item("Heavy Oil Residue").Volume(1)),
            Item.List(new Item("Smokeless Powder").Volume(2))),
        new Recipe("Sulfuric Acid", new Duration(6),
            Refinery,
            Item.List(
                new Item("Sulfur").Volume(5),
                new Item("Water").Volume(5)),
            Item.List(new Item("Sulfuric Acid").Volume(5))),

        new Recipe("Turbofuel", new Duration(16),
            Refinery,
            Item.List(
                new Item("Fuel").Volume(6),
                new Item("Compacted Coal").Volume(4)),
            Item.List(new Item("Turbofuel").Volume(5))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
        new Recipe("Coated Cable", new Duration(8),
            Refinery,
            Item.List(
                new Item("Wire").Volume(5),
                new Item("Heavy Oil Residue").Volume(1)),
            Item.List(new Item("Cable").Volume(9)),
            RecipeFlags.Alternate),

        new Recipe("Diluted Packaged Fuel", new Duration(2),
            Refinery,
            Item.List(
                new Item("Heavy Oil Residue").Volume(1),
                new Item("Packaged Water").Volume(2)),
            Item.List(new Item("Packaged Fuel").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Electrode Aluminum Scrap", new Duration(4),
            Refinery,
            Item.List(
                new Item("Alumina Solution").Volume(12),
                new Item("Petroleum Coke").Volume(4)),
            Item.List(
                new Item("Aluminum Scrap").Volume(20),
                new Item("Water").Volume(7)),
            RecipeFlags.Alternate),

        new Recipe("Heavy Oil Residue", new Duration(6),
            Refinery,
            Item.List(new Item("Crude Oil").Volume(3)),
            Item.List(
                new Item("Heavy Oil Residue").Volume(4),
                new Item("Polymer Resin").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Polymer Resin", new Duration(6),
            Refinery,
            Item.List(new Item("Crude Oil").Volume(6)),
            Item.List(
                new Item("Polymer Resin").Volume(13),
                new Item("Heavy Oil Residue").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Pure Caterium Ingot", new Duration(5),
            Refinery,
            Item.List(
                new Item("Caterium Ore").Volume(2),
                new Item("Water").Volume(2)),
            Item.List(new Item("Caterium Ingot").Volume(1)),
            RecipeFlags.Alternate),
        new Recipe("Pure Copper Ingot", new Duration(24),
            Refinery,
            Item.List(
                new Item("Copper Ore").Volume(6),
                new Item("Water").Volume(4)),
            Item.List(new Item("Copper Ingot").Volume(15)),
            RecipeFlags.Alternate),
        new Recipe("Pure Iron Ingot", new Duration(12),
            Refinery,
            Item.List(
                new Item("Iron Ore").Volume(7),
                new Item("Water").Volume(4)),
            Item.List(new Item("Iron Ingot").Volume(13)),
            RecipeFlags.Alternate),
        new Recipe("Pure Quartz Crystal", new Duration(8),
            Refinery,
            Item.List(
                new Item("Raw Quartz").Volume(9),
                new Item("Water").Volume(5)),
            Item.List(new Item("Quartz Crystal").Volume(7)),
            RecipeFlags.Alternate),

        new Recipe("Recycled Plastic", new Duration(12),
            Refinery,
            Item.List(
                new Item("Rubber").Volume(6),
                new Item("Fuel").Volume(6)),
            Item.List(new Item("Plastic").Volume(12)),
            RecipeFlags.Alternate),
        new Recipe("Recycled Rubber", new Duration(12),
            Refinery,
            Item.List(
                new Item("Plastic").Volume(6),
                new Item("Fuel").Volume(6)),
            Item.List(new Item("Rubber").Volume(12)),
            RecipeFlags.Alternate),

        new Recipe("Sloppy Alumina", new Duration(3),
            Refinery,
            Item.List(
                new Item("Bauxite").Volume(10),
                new Item("Water").Volume(10)),
            Item.List(new Item("Alumina Solution").Volume(12)),
            RecipeFlags.Alternate),
        new Recipe("Steamed Copper Sheet", new Duration(8),
            Refinery,
            Item.List(
                new Item("Copper Ingot").Volume(3),
                new Item("Water").Volume(3)),
            Item.List(new Item("Copper Sheet").Volume(3)),
            RecipeFlags.Alternate),

        new Recipe("Turbo Heavy Fuel", new Duration(8),
            Refinery,
            Item.List(
                new Item("Heavy Oil Residue").Volume(5),
                new Item("Compacted Coal").Volume(4)),
            Item.List(new Item("Turbofuel").Volume(4)),
            RecipeFlags.Alternate),

        new Recipe("Wet Concrete", new Duration(3),
            Refinery,
            Item.List(
                new Item("Limestone").Volume(6),
                new Item("Water").Volume(5)),
            Item.List(new Item("Concrete").Volume(4)),
            RecipeFlags.Alternate),
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
