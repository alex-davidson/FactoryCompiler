using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.DefaultGameDataFiles;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.Model;

/// <summary>
/// Verify that the source data structures follow consistent rules.
/// </summary>
[TestFixture]
public class DefaultGameDataFilesTests
{
    [Datapoints]
    public static IFactoryData[] FactoryDataCases { get; } =
    {
        new MinerMk1Data(),
        new MinerMk2Data(),
        new MinerMk3Data(),
        new WaterExtractorData(),
        new OilExtractorData(),

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

    [Theory]
    public void AllBasicRecipesHaveNoFlags(IFactoryData factoryData)
    {
        // Check that every recipe in the Recipes list is not marked as Alternate, etc.
        var violations = factoryData.Recipes.Where(x => x.Flags != RecipeFlags.None).ToArray();
        Assert.That(violations, Is.Empty);
    }

    [Theory]
    public void AllAlternateRecipesAreMarkedAsSuch(IFactoryData factoryData)
    {
        // Check that every recipe in the AlternateRecipes list is marked as such.
        var violations = factoryData.AlternateRecipes.Where(x => !x.Flags.HasFlag(RecipeFlags.Alternate)).ToArray();
        Assert.That(violations, Is.Empty);
    }

    [Theory]
    public void AllProjectAssemblyRecipesAreMarkedAsSuch(IFactoryData factoryData)
    {
        // Check that every recipe in the ProjectAssemblyRecipes list is marked as such.
        var violations = factoryData.ProjectAssemblyRecipes.Where(x => !x.Flags.HasFlag(RecipeFlags.ProjectAssembly)).ToArray();
        Assert.That(violations, Is.Empty);
    }

    [Theory]
    public void AllProjectAssemblyRecipesAreInProjectAssemblyRecipesList(IFactoryData factoryData)
    {
        // If a recipe is both ProjectAssembly and Alternate, it should be in ProjectAssemblyRecipes.
        var violations = factoryData.AllRecipes.Except(factoryData.ProjectAssemblyRecipes)
            .Where(x => x.Flags.HasFlag(RecipeFlags.ProjectAssembly))
            .ToArray();
        Assert.That(violations, Is.Empty);
    }

    [Theory]
    public void AllRecipesAreMadeByFactory(IFactoryData factoryData)
    {
        // Check that every recipe in this data set is made by this factory.
        var violations = factoryData.AllRecipes.Where(x => !x.MadeByFactory.Equals(factoryData.Factory)).ToArray();
        Assert.That(violations, Is.Empty);
    }
}
