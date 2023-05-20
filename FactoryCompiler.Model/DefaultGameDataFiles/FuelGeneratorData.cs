namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class FuelGeneratorData : DefaultGameDataBase, IFactoryData
{
    public static Factory FuelGenerator { get; } = new Factory("Fuel Generator");

    public Factory Factory => FuelGenerator;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Fuel", new Duration(60),
            FuelGenerator,
            Item.List(new Item("Fuel").Volume(12)),
            Item.NoItems),
        new Recipe("Liquid Biofuel", new Duration(60),
            FuelGenerator,
            Item.List(new Item("Liquid Biofuel").Volume(12)),
            Item.NoItems),

        new Recipe("Turbofuel", new Duration(60),
            FuelGenerator,
            Item.List(
                new Item("Turbofuel").Volume(9 / 2)),
            Item.NoItems),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
