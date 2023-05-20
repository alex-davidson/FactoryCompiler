using Rationals;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class CoalGeneratorData : DefaultGameDataBase, IFactoryData
{
    public static Factory CoalGenerator { get; } = new Factory("Coal Generator");

    public Factory Factory => CoalGenerator;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Coal", new Duration(60),
            CoalGenerator,
            Item.List(
                new Item("Coal").Volume(15),
                new Item("Water").Volume(45)),
            Item.NoItems),
        new Recipe("Compacted Coal", new Duration(60),
            CoalGenerator,
            Item.List(
                new Item("Compacted Coal").Volume(60 / Rational.Approximate(8.4)), // ~7.142
                new Item("Water").Volume(45)),
            Item.NoItems),

        new Recipe("Petroleum Coke", new Duration(60),
            CoalGenerator,
            Item.List(
                new Item("Petroleum Coke").Volume(25),
                new Item("Water").Volume(45)),
            Item.NoItems),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
