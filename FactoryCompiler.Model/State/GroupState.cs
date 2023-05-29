using System.Collections.Generic;
using System.Collections.Immutable;

namespace FactoryCompiler.Model.State;

public class GroupState
{
    public Group Definition { get; }
    public Identifier? GroupName { get; private init; }
    public ProductionState? Production { get; }
    public ImmutableArray<GroupState> Groups { get; }
    public ItemVolumesState ItemVolumes { get; set; }
    public int ParentRepeats { get; }

    public string GetPreferredName() => GroupName?.Name ?? Production?.Recipe.RecipeName.Name ?? "";

    private GroupState(Group definition, ProductionState? production, ImmutableArray<GroupState> groups, int parentRepeats)
    {
        Definition = definition;
        GroupName = definition.GroupName;
        Production = production;
        Groups = groups;
        ParentRepeats = parentRepeats;
        ItemVolumes = new ItemVolumesState();
    }

    public GroupState(Group definition, ProductionState production, int parentRepeats) : this(definition, production, ImmutableArray<GroupState>.Empty, parentRepeats)
    {
    }

    public GroupState(Group definition, ImmutableArray<GroupState> groups, int parentRepeats) : this(definition, null, groups, parentRepeats)
    {
    }

    public GroupState WithGroupName(Identifier groupName)
    {
        return new GroupState(Definition, Production, Groups, ParentRepeats)
        {
            GroupName = groupName,
        };
    }

    public GroupState WithParentRepeats(int repeats)
    {
        return new GroupState(Definition, Production, Groups, repeats)
        {
            GroupName = GroupName,
        };
    }
}
