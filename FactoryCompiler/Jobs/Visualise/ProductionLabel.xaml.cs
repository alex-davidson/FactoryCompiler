using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using FactoryCompiler.Model;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Summary description of a Region in the graph view.
/// </summary>
public partial class ProductionLabel : UserControl
{
    public ProductionLabel(ProductionLabelModel model)
    {
        DataContext = model;
        InitializeComponent();
    }
}

public class ProductionLabelModel
{
    private readonly FactoryModel? model;
    private readonly GroupState? groupState;

    public ProductionLabelModel()
    {
    }

    public ProductionLabelModel(FactoryModel? model, GroupState groupState)
    {
        this.model = model;
        this.groupState = groupState;
    }

    public string Name => groupState?.GetPreferredName() ?? "";
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
