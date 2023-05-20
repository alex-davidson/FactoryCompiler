using System.Collections.Generic;
using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.DefaultGameDataFiles;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.Model;

[TestFixture]
public class DefaultGameDataTests
{
    private static readonly IGameData sut = new DefaultGameData().Build();

    [Datapoints]
    public static IEnumerable<Item> Items => sut.AllItems;
    [Datapoints]
    public static IEnumerable<Factory> Factories => sut.Factories;
    [Datapoints]
    public static IEnumerable<Recipe> Recipes => sut.Recipes;
    [Datapoints]
    public static IEnumerable<Identifier> AllIdentifiers => sut.AllItems.Select(x => x.Identifier)
        .Union(sut.Recipes.Select(x => x.RecipeName))
        .Union(sut.Factories.Select(x => x.FactoryName));

    public static IEnumerable<Item> ManufacturedItems => Items.Except(new NaturalResources().NaturalResourceItems);
    public static IEnumerable<Recipe> BasicRecipes => sut.Recipes.Where(r => !r.Flags.HasFlag(RecipeFlags.Alternate));

    [Theory]
    public void RecipeIsValid(Recipe recipe)
    {
        Assert.That(Items, Is.SupersetOf(recipe.Inputs.Select(i => i.Item)),
            "One or more input items are not declared (or specified incorrectly) for recipe {0}", recipe.RecipeName);
        Assert.That(Items, Is.SupersetOf(recipe.Outputs.Select(o => o.Item)),
            "One or more output items are not declared (or specified incorrectly) for recipe {0}", recipe.RecipeName);
    }

    [Theory]
    public void FactoryExistsForRecipe(Recipe recipe)
    {
        Assert.That(Items, Is.SupersetOf(recipe.Inputs.Select(i => i.Item)),
            "One or more input items are not declared (or specified incorrectly) for recipe {0}", recipe.RecipeName);
        Assert.That(Items, Is.SupersetOf(recipe.Outputs.Select(o => o.Item)),
            "One or more output items are not declared (or specified incorrectly) for recipe {0}", recipe.RecipeName);
    }

    [TestCaseSource(nameof(ManufacturedItems))]
    public void AllManufacturedItemsHaveBasicRecipes(Item item)
    {
        // Exception: usually made manually.
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Portable Miner")));

        Assert.That(BasicRecipes.Where(r => r.Outputs.Select(i => i.Item).Contains(item)), Is.Not.Empty);
    }

    [TestCaseSource(nameof(ManufacturedItems))]
    public void ManufacturedItemsShareANameWithABasicRecipe(Item item)
    {
        // Exception: usually made manually.
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Portable Miner")));
        // Exceptions: weird cases.
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Alien Protein")));
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Biomass")));
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Power Shard")));
        // Exceptions: recipe with same name is an Alternate.
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Heavy Oil Residue")));
        Assume.That(item.Identifier, Is.Not.EqualTo(new Identifier("Polymer Resin")));

        Assert.That(BasicRecipes.Where(r => r.Outputs.Select(x => x.Item).Contains(item)).Select(r => r.RecipeName), Does.Contain(item.Identifier));
    }

    [Theory]
    public void SpellingOfAluminiumIsConsistent(Identifier identifier)
    {
        // I'd rather spell it like this, but the game does not. Must standardise on 'aluminum' and fix it at render time.
        Assert.That(identifier.Name, Does.Not.Contain("aluminium").IgnoreCase);
    }

    [Theory]
    public void SpellingOfSulphurIsConsistent(Identifier identifier)
    {
        Assert.That(identifier.Name, Does.Not.Contain("sulphur").IgnoreCase);
    }
}
