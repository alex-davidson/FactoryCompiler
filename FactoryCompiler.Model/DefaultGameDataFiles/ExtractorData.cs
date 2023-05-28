using System.Collections.Generic;
using System.Linq;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

internal class ExtractorData : DefaultGameDataBase, IFactoryData
{
    public static Factory Extractor { get; } = new Factory("Extractor");

    public Factory Factory => Extractor;

    private static readonly Item[] resourceTypes =
    {
        new Item("Nitrogen Gas"),
        new Item("Water"),
        new Item("Crude Oil"),
    };

    private static IEnumerable<Recipe> Extracting(Item ore)
    {
        yield return new Recipe($"Impure {ore.Identifier.Name}", new Duration(60),
            Extractor,
            Item.NoItems,
            Item.List(ore.Volume(30)));
        yield return new Recipe($"Normal {ore.Identifier.Name}", new Duration(60),
            Extractor,
            Item.NoItems,
            Item.List(ore.Volume(60)));
        yield return new Recipe($"Pure {ore.Identifier.Name}", new Duration(60),
            Extractor,
            Item.NoItems,
            Item.List(ore.Volume(120)));
    }

    public Recipe[] Recipes { get; } =
        resourceTypes.SelectMany(Extracting).ToArray();

    public Recipe[] AlternateRecipes { get; } =
    {
    };

    public Recipe[] ProjectAssemblyRecipes { get; } =
    {
    };
}
