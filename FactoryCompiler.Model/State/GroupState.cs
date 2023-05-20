using System.Collections.Generic;
using System.Collections.Immutable;

namespace FactoryCompiler.Model.State;

public class GroupState
{
    public Group Definition { get; }
    public Identifier? GroupName { get; private set; }
    public ProductionState? Production { get; }
    public ImmutableArray<GroupState> Groups { get; }
    public ItemVolumesState ItemVolumes { get; set; }

    private GroupState(Group definition, ProductionState? production, ImmutableArray<GroupState> groups)
    {
        Definition = definition;
        GroupName = definition.GroupName;
        Production = production;
        Groups = groups;
        ItemVolumes = new ItemVolumesState();
    }

    public GroupState(Group definition, ProductionState production) : this(definition, production, ImmutableArray<GroupState>.Empty)
    {
    }

    public GroupState(Group definition, ImmutableArray<GroupState> groups) : this(definition, null, groups)
    {
    }

    public GroupState WithGroupName(Identifier groupName)
    {
        return new GroupState(Definition, Production, Groups)
        {
            GroupName = groupName,
        };
    }
}
