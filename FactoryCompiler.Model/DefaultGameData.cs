using System.Collections.Immutable;
using System.Linq;
using FactoryCompiler.Model.DefaultGameDataFiles;

namespace FactoryCompiler.Model;

internal class DefaultGameData : DefaultGameDataBase
{
    private static readonly IFactoryData[] factoryData =
    {
        new SmelterData(),
        new FoundryData(),

        new ConstructorData(),
        new AssemblerData(),
        new ManufacturerData(),

        new PackagerData(),
        new RefineryData(),
        new BlenderData(),
        new ParticleAcceleratorData(),

        new CoalGeneratorData(),
        new FuelGeneratorData(),
        new NuclearPowerPlantData(),
    };

    public ImmutableList<Factory> Factories => factoryData.Select(x => x.Factory).ToImmutableList();
    public ImmutableList<Recipe> Recipes { get; } = factoryData.SelectMany(x => x.AllRecipes).ToImmutableList();

    public IGameData Build()
    {
        var builder = new GameDataBuilder();
        foreach (var recipe in Recipes) builder.Recipes.Add(recipe);
        foreach (var factory in Factories) builder.Factories.Add(factory);
        return builder.Build();
    }
}
