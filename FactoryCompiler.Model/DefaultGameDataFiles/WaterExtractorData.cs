namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class WaterExtractorData : DefaultGameDataBase, IFactoryData
{
    public static Factory WaterExtractor { get; } = new Factory("Water Extractor");

    public Factory Factory => WaterExtractor;

    public Recipe[] Recipes { get; } =
    {
        new Recipe("Water", new Duration(60),
            WaterExtractor,
            Item.NoItems,
            Item.List(new Item("Water").Volume(120))),
    };

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
