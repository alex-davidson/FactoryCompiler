using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class BlenderData : DefaultGameDataBase, IFactoryData
{
    public static Factory Blender { get; } = new Factory("Blender");

    public Factory Factory => Blender;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Battery", new Duration(3),
            Blender,
            Item.List(
                new Item("Sulfuric Acid").Volume(5 / 2),
                new Item("Alumina Solution").Volume(2),
                new Item("Aluminum Casing").Volume(1)),
            Item.List(
                new Item("Battery").Volume(1),
                new Item("Water").Volume(3 / 2))),

        new Recipe("Cooling System", new Duration(10),
            Blender,
            Item.List(
                new Item("Heat Sink").Volume(2),
                new Item("Rubber").Volume(2),
                new Item("Water").Volume(5),
                new Item("Nitrogen Gas").Volume(25)),
            Item.List(new Item("Cooling System").Volume(1))),

        new Recipe("Encased Uranium Cell", new Duration(12),
            Blender,
            Item.List(
                new Item("Uranium").Volume(10),
                new Item("Concrete").Volume(3),
                new Item("Sulfuric Acid").Volume(8)),
            Item.List(
                new Item("Encased Uranium Cell").Volume(5),
                new Item("Sulfuric Acid").Volume(2))),

        new Recipe("Fused Modular Frame", new Duration(40),
            Blender,
            Item.List(
                new Item("Heavy Modular Frame").Volume(1),
                new Item("Aluminum Casing").Volume(50),
                new Item("Nitrogen Gas").Volume(25)),
            Item.List(new Item("Fused Modular Frame").Volume(1))),

        new Recipe("Nitric Acid", new Duration(6),
            Blender,
            Item.List(
                new Item("Nitrogen Gas").Volume(12),
                new Item("Water").Volume(3),
                new Item("Iron Plate").Volume(1)),
            Item.List(new Item("Nitric Acid").Volume(3))),

        new Recipe("Non-fissile Uranium", new Duration(24),
            Blender,
            Item.List(
                new Item("Uranium Waste").Volume(15),
                new Item("Silica").Volume(10),
                new Item("Nitric Acid").Volume(6),
                new Item("Sulfuric Acid").Volume(6)),
            Item.List(
                new Item("Non-fissile Uranium").Volume(20),
                new Item("Water").Volume(6))),

        new Recipe("Turbo Rifle Ammo", new Duration(12),
            Blender,
            Item.List(
                new Item("Rifle Ammo").Volume(25),
                new Item("Aluminum Casing").Volume(3),
                new Item("Turbofuel").Volume(3)),
            Item.List(new Item("Turbo Rifle Ammo").Volume(50))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
        new Recipe("Cooling Device", new Duration(32),
            Blender,
            Item.List(
                new Item("Heat Sink").Volume(5),
                new Item("Motor").Volume(1),
                new Item("Nitrogen Gas").Volume(24)),
            Item.List(new Item("Cooling System").Volume(2)),
            RecipeFlags.Alternate),

        new Recipe("Diluted Fuel", new Duration(6),
            Blender,
            Item.List(
                new Item("Heavy Oil Residue").Volume(5),
                new Item("Water").Volume(10)),
            Item.List(new Item("Fuel").Volume(10)),
            RecipeFlags.Alternate),

        new Recipe("Fertile Uranium", new Duration(12),
            Blender,
            Item.List(
                new Item("Uranium").Volume(5),
                new Item("Uranium Waste").Volume(5),
                new Item("Nitric Acid").Volume(3),
                new Item("Sulfuric Acid").Volume(5)),
            Item.List(
                new Item("Non-fissile Uranium").Volume(20),
                new Item("Water").Volume(8)),
            RecipeFlags.Alternate),

        new Recipe("Heat-Fused Frame", new Duration(20),
            Blender,
            Item.List(
                new Item("Heavy Modular Frame").Volume(1),
                new Item("Aluminum Ingot").Volume(50),
                new Item("Nitric Acid").Volume(8),
                new Item("Fuel").Volume(10)),
            Item.List(
                new Item("Fused Modular Frame").Volume(1)),
            RecipeFlags.Alternate),

        new Recipe("Instant Scrap", new Duration(6),
            Blender,
            Item.List(
                new Item("Bauxite").Volume(15),
                new Item("Coal").Volume(10),
                new Item("Sulfuric Acid").Volume(5),
                new Item("Water").Volume(6)),
            Item.List(
                new Item("Aluminum Scrap").Volume(30),
                new Item("Water").Volume(5)),
            RecipeFlags.Alternate),

        new Recipe("Turbo Blend Fuel", new Duration(8),
            Blender,
            Item.List(
                new Item("Fuel").Volume(2),
                new Item("Heavy Oil Residue").Volume(4),
                new Item("Sulfur").Volume(3),
                new Item("Petroleum Coke").Volume(3)),
            Item.List(new Item("Turbofuel").Volume(6)),
            RecipeFlags.Alternate),
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
