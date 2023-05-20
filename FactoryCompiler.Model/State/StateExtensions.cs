using System.Collections.Generic;

namespace FactoryCompiler.Model.State;

internal static class StateExtensions
{
    public static IEnumerable<ItemVolumesState> GetChildStates(this FactoryState factory)
    {
        foreach (var region in factory.Regions)
        {
            yield return region.ItemVolumes;
        }
    }

    public static IEnumerable<ItemVolumesState> GetChildStates(this RegionState region)
    {
        foreach (var child in region.Groups)
        {
            yield return child.ItemVolumes;
        }
    }

    public static IEnumerable<ItemVolumesState> GetChildStates(this GroupState group)
    {
        if (group.Production != null) yield return group.Production.ItemVolumes;
        foreach (var child in group.Groups)
        {
            yield return child.ItemVolumes;
        }
    }
}
