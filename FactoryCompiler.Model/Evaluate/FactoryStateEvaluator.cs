using System;
using FactoryCompiler.Model.State;
using Rationals;

namespace FactoryCompiler.Model.Evaluate;

public class FactoryStateEvaluator
{
    public FactoryStateEvaluator() : this(TimeSpan.FromMinutes(1))
    {
    }

    public FactoryStateEvaluator(TimeSpan timeStep)
    {
        TimeStep = timeStep;
    }

    public TimeSpan TimeStep { get; }

    public void UpdateInPlace(FactoryState factory)
    {
        foreach (var region in factory.Regions)
        {
            UpdateInPlace(region);
        }
        factory.ItemVolumes = ItemVolumesState.Sum(factory.GetChildStates());
    }

    private void UpdateInPlace(RegionState region)
    {
        foreach (var child in region.Groups)
        {
            UpdateInPlace(child, 1);
        }
        region.ItemVolumes = ItemVolumesState.Sum(region.GetChildStates());
    }

    private void UpdateInPlace(GroupState group, int scale)
    {
        var repeatScale = scale * group.Definition.Repeat;
        if (group.Production != null)
        {
            UpdateInPlace(group.Production, repeatScale);
        }
        foreach (var child in group.Groups)
        {
            UpdateInPlace(child, repeatScale);
        }
        group.ItemVolumes = ItemVolumesState.Sum(group.GetChildStates());
    }

    private void UpdateInPlace(ProductionState production, int scale)
    {
        production.ItemVolumes.Reset();
        var perTimeStep = Rational.Approximate(TimeStep.TotalSeconds) * production.PerSecond * scale;
        foreach (var input in production.Recipe.Inputs)
        {
            production.ItemVolumes.Consume(input.Item, input.Volume * perTimeStep);
        }
        foreach (var output in production.Recipe.Outputs)
        {
            production.ItemVolumes.Produce(output.Item, output.Volume * perTimeStep);
        }
    }
}
