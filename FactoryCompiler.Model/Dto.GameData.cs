namespace FactoryCompiler.Model;

public static partial class Dto
{
    public class GameData
    {
        public Recipe[]? Recipes { get; set; }
        public Factory[]? Factories { get; set; }

        public class Factory
        {
            public string? FactoryName { get; set; }
        }

        public class Recipe
        {
            public string? RecipeName { get; set; }
            public string? BaseDuration { get; set; }
            public string? MadeByFactory { get; set; }
            public ItemVolume[]? Inputs { get; set; }
            public ItemVolume[]? Outputs { get; set; }
            public bool IsAlternate { get; set; }
            public bool IsProjectAssembly { get; set; }
            public bool IsFICSMAS { get; set; }
        }

        public class ItemVolume
        {
            public string? Item { get; set; }
            public string? Volume { get; set; }
        }
    }
}
