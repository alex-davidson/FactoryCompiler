using FactoryCompiler.Model.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace FactoryCompiler.Model.Mappers;

internal class GameDataModelValidator
{
    public ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();

    public void Validate(Dto.GameData dto)
    {
        if (dto.Recipes != null)
        {
            for (var index = 0; index < dto.Recipes.Length; index++)
            {
                Validate(dto.Recipes[index], index);
            }
        }

        if (dto.Factories != null)
        {
            for (var index = 0; index < dto.Factories.Length; index++)
            {
                Validate(dto.Factories[index], index);
            }
        }
    }

    private void Validate(Dto.GameData.Recipe recipe, int index)
    {
        var name = string.IsNullOrWhiteSpace(recipe.RecipeName) ? $"at index {index}" : $"'{recipe.RecipeName}'";
        if (string.IsNullOrWhiteSpace(recipe.RecipeName))
        {
            Diagnostics.Add(Diagnostic.Error($"Recipe {name} does not specify {nameof(recipe.RecipeName)}."));
        }
        if (recipe.MadeByFactory == null)
        {
            Diagnostics.Add(Diagnostic.Error($"Recipe {name} does not specify {nameof(recipe.MadeByFactory)}."));
        }
        if (string.IsNullOrWhiteSpace(recipe.BaseDuration))
        {
            Diagnostics.Add(Diagnostic.Error($"Recipe {name} does not specify {nameof(recipe.BaseDuration)}."));
        }
        if (recipe.Inputs?.Any() == true)
        {
            if (recipe.Inputs.Any(i => string.IsNullOrWhiteSpace(i.Item)))
            {
                Diagnostics.Add(Diagnostic.Error($"Recipe {name}: one or more inputs do not specify {nameof(ItemVolume.Item)}."));
            }
            if (recipe.Inputs.Any(i => string.IsNullOrWhiteSpace(i.Volume)))
            {
                Diagnostics.Add(Diagnostic.Warning($"Recipe {name}: one or more inputs do not specify {nameof(ItemVolume.Volume)}. '1' will be assumed."));
            }
        }

        if (recipe.Outputs?.Any() == true)
        {
            if (recipe.Outputs.Any(i => string.IsNullOrWhiteSpace(i.Item)))
            {
                Diagnostics.Add(Diagnostic.Error($"Recipe {name}: one or more outputs do not specify {nameof(ItemVolume.Item)}."));
            }
            if (recipe.Outputs.Any(i => string.IsNullOrWhiteSpace(i.Volume)))
            {
                Diagnostics.Add(Diagnostic.Warning($"Recipe {name}: one or more outputs do not specify {nameof(ItemVolume.Volume)}. '1' will be assumed."));
            }
        }
    }

    private void Validate(Dto.GameData.Factory factory, int index)
    {
        var name = string.IsNullOrWhiteSpace(factory.FactoryName) ? $"at index {index}" : $"'{factory.FactoryName}'";
        if (string.IsNullOrWhiteSpace(factory.FactoryName))
        {
            Diagnostics.Add(Diagnostic.Error($"Factory {name} does not specify {nameof(factory.FactoryName)}."));
        }
    }
}
