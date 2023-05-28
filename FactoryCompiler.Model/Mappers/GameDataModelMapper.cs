using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FactoryCompiler.Model.Mappers;

internal struct GameDataModelMapper
{
    public Dto.GameData MapToDto(IGameData gameData) =>
        new Dto.GameData
        {
            Recipes = gameData.Recipes.Select(default(RecipeMapper).MapToDto).ToArray(),
            Factories = gameData.Factories.Select(default(FactoryMapper).MapToDto).ToArray(),
        };

    public IGameData MapFromDto(Dto.GameData dto)
    {
        var builder = new GameDataBuilder();
        foreach (var recipe in MapListFrom(dto.Recipes?.Select(default(RecipeMapper).MapFromDto)))
        {
            builder.Recipes.Add(recipe);
        }
        foreach (var factory in MapListFrom(dto.Factories?.Select(default(FactoryMapper).MapFromDto)))
        {
            builder.Factories.Add(factory);
        }
        return builder.Build();
    }

    private static ImmutableArray<T> MapListFrom<T>(IEnumerable<T>? list)
    {
        if (list == null) return ImmutableArray<T>.Empty;
        return list.ToImmutableArray();
    }

    private struct RecipeMapper
    {
        public Dto.GameData.Recipe MapToDto(Recipe recipe) =>
            new Dto.GameData.Recipe
            {
                RecipeName = recipe.RecipeName.Name,
                BaseDuration = default(NumberMapper).MapToDecimalDto(recipe.BaseDuration),
                MadeByFactory = recipe.MadeByFactory.FactoryName.Name,
                Inputs = recipe.Inputs.Select(default(ItemVolumeMapper).MapToDto).ToArray(),
                Outputs = recipe.Outputs.Select(default(ItemVolumeMapper).MapToDto).ToArray(),
                IsAlternate = recipe.Flags.HasFlag(RecipeFlags.Alternate),
                IsProjectAssembly = recipe.Flags.HasFlag(RecipeFlags.ProjectAssembly),
                IsFICSMAS = recipe.Flags.HasFlag(RecipeFlags.FICSMAS),
            };

        public Recipe MapFromDto(Dto.GameData.Recipe dto) =>
            new Recipe(
                RecipeName: dto.RecipeName!,
                BaseDuration: default(NumberMapper).MapDurationFromDto(dto.BaseDuration!),
                MadeByFactory: new Factory(dto.MadeByFactory!),
                Inputs: MapListFrom(dto.Inputs?.Select(default(ItemVolumeMapper).MapFromDto)),
                Outputs: MapListFrom(dto.Outputs?.Select(default(ItemVolumeMapper).MapFromDto)),
                Flags: (dto.IsAlternate ? RecipeFlags.Alternate : RecipeFlags.None) |
                       (dto.IsProjectAssembly ? RecipeFlags.ProjectAssembly : RecipeFlags.None) |
                       (dto.IsFICSMAS ? RecipeFlags.FICSMAS : RecipeFlags.None));
    }

    private struct FactoryMapper
    {
        public Dto.GameData.Factory MapToDto(Factory factory) =>
            new Dto.GameData.Factory
            {
                FactoryName = factory.FactoryName.Name,
            };

        public Factory MapFromDto(Dto.GameData.Factory dto) =>
            new Factory(dto.FactoryName!);
    }

    private struct ItemMapper
    {
        public string MapToDto(Item item) => item.Identifier.Name;
        public Item MapFromDto(string dto) => new Item(dto);
    }

    private struct ItemVolumeMapper
    {
        public Dto.GameData.ItemVolume MapToDto(ItemVolume itemVolume) =>
            new Dto.GameData.ItemVolume
            {
                Item = default(ItemMapper).MapToDto(itemVolume.Item),
                Volume = default(NumberMapper).MapToDecimalDto(itemVolume.Volume),
            };

        public ItemVolume MapFromDto(Dto.GameData.ItemVolume dto) =>
            new ItemVolume(
                item: default(ItemMapper).MapFromDto(dto.Item!),
                volume: default(NumberMapper).MapFromDto(dto.Volume, 1));
    }
}
