using System.Reflection;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class OilExtractorData : DefaultGameDataBase, IFactoryData
{
    public static Factory OilExtractor { get; } = new Factory("Oil Extractor");

    public Factory Factory => OilExtractor;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Impure Crude Oil", new Duration(60),
            OilExtractor,
            Item.NoItems,
            Item.List(new Item("Crude Oil").Volume(60))),
        new Recipe("Normal Crude Oil", new Duration(60),
            OilExtractor,
            Item.NoItems,
            Item.List(new Item("Crude Oil").Volume(120))),
        new Recipe("Pure Crude Oil", new Duration(60),
            OilExtractor,
            Item.NoItems,
            Item.List(new Item("Crude Oil").Volume(240))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
