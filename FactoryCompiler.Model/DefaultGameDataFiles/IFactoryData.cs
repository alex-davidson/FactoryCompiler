using System.Collections.Generic;
using System.Linq;

namespace FactoryCompiler.Model.DefaultGameDataFiles;

public interface IFactoryData
{
    Factory Factory { get; }
    Recipe[] Recipes { get; }
    Recipe[] AlternateRecipes { get; }
    Recipe[] ProjectAssemblyRecipes { get; }
    IEnumerable<Recipe> AllRecipes => Recipes.Concat(AlternateRecipes).Concat(ProjectAssemblyRecipes);
}
