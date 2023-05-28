using System.Linq;
using System.Windows.Controls;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Summary description of a Region in the graph view.
/// </summary>
public partial class RegionLabel : UserControl
{
    public RegionLabel(RegionLabelModel model)
    {
        DataContext = model;
        InitializeComponent();
    }
}

public class RegionLabelModel
{
    private readonly FactoryModel? model;
    private readonly Region? region;

    public RegionLabelModel()
    {
    }

    public RegionLabelModel(FactoryModel? model, Region region)
    {
        this.model = model;
        this.region = region;
    }

    public string Name => region?.RegionName.Name ?? "";
    public IItemVolumesState State
    {
        get
        {
            if (model?.Summary == null) return ItemVolumesState.Empty;
            if (region == null) return ItemVolumesState.Empty;
            return model.Summary.GetRegion(region);
        }
    }

    public SurplusAndShortfallModel Summary => new SurplusAndShortfallModel(ItemVolumeSummary.Create(State));
}
