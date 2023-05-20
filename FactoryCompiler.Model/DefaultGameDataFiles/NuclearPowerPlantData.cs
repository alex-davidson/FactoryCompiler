namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class NuclearPowerPlantData : DefaultGameDataBase, IFactoryData
{
    public static Factory NuclearPowerPlant { get; } = new Factory("Nuclear Power Plant");

    public Factory Factory => NuclearPowerPlant;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Plutonium Waste", new Duration(600),
            NuclearPowerPlant,
            Item.List(
                new Item("Plutonium Fuel Rod").Volume(1),
                new Item("Water").Volume(2400)),
            Item.List(new Item("Plutonium Waste").Volume(10))),

        new Recipe("Uranium Waste", new Duration(300),
            NuclearPowerPlant,
            Item.List(
                new Item("Uranium Fuel Rod").Volume(1),
                new Item("Water").Volume(1200)),
            Item.List(new Item("Uranium Waste").Volume(50))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
