using Rationals;

namespace FactoryCompiler.Model.State;

public class ProductionState
{
    public Production Definition { get; }
    public Recipe Recipe { get; }
    public ItemVolumesState ItemVolumes { get; }
    public Rational PerSecond { get; }

    public ProductionState(Production definition, Recipe recipe)
    {
        Definition = definition;
        Recipe = recipe;
        ItemVolumes = new ItemVolumesState();
        PerSecond = definition.Count / recipe.BaseDuration;
    }
}
