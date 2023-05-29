using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using FactoryCompiler.Model;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Summary description of a Group in the graph view.
/// </summary>
public partial class GroupLabel : UserControl
{
    public GroupLabel(GroupLabelModel model)
    {
        DataContext = model;
        InitializeComponent();
    }
}

public class GroupLabelModel
{
    private readonly FactoryModel? model;
    private readonly GroupState? groupState;

    public GroupLabelModel()
    {
    }

    public GroupLabelModel(FactoryModel? model, GroupState groupState)
    {
        this.model = model;
        this.groupState = groupState;
    }

    public string Name => groupState?.GroupName?.Name ?? "";
    public string Repeat => groupState?.Definition.Repeat > 1 ? $" x{groupState?.Definition.Repeat}" : "";
    public IItemVolumesState State => groupState?.ItemVolumes ?? ItemVolumesState.Empty;

    public SurplusAndShortfallModel Summary
    {
        get
        {
            if (groupState == null) return default;
            if (model?.Summary == null) return default;
            var state = model.Summary.GetGroup(groupState.Definition);
            return new SurplusAndShortfallModel(state.GetScaledSummary());
        }
    }
}
