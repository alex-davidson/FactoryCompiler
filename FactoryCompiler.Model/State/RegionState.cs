using System.Collections.Immutable;

namespace FactoryCompiler.Model.State;

public class RegionState
{
    public Region Definition { get; }
    public Identifier RegionName => Definition.RegionName;
    public ImmutableArray<GroupState> Groups { get; }
    public ItemVolumesState ItemVolumes { get; set; }

    public RegionState(Region definition, ImmutableArray<GroupState> groups)
    {
        Definition = definition;
        Groups = groups;
        ItemVolumes = new ItemVolumesState();
    }
}
