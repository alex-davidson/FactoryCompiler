using System.Collections.Immutable;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class ParticleAcceleratorData : DefaultGameDataBase, IFactoryData
{
    public static Factory ParticleAccelerator { get; } = new Factory("Particle Accelerator");

    public Factory Factory => ParticleAccelerator;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Nuclear Pasta", new Duration(120),
            ParticleAccelerator,
            Item.List(
                new Item("Copper Powder").Volume(200),
                new Item("Pressure Conversion Cube").Volume(1)),
            Item.List(new Item("Nuclear Pasta").Volume(1))),
        new Recipe("Plutonium Pellet", new Duration(60),
            ParticleAccelerator,
            Item.List(
                new Item("Non-fissile Uranium").Volume(100),
                new Item("Uranium Waste").Volume(25)),
            Item.List(new Item("Plutonium Pellet").Volume(30))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
        new Recipe("Instant Plutonium Cell", new Duration(120),
            ParticleAccelerator,
            Item.List(
                new Item("Non-fissile Uranium").Volume(150),
                new Item("Aluminum Casing").Volume(20)),
            Item.List(new Item("Encased Plutonium Cell").Volume(20)),
            RecipeFlags.Alternate),
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
