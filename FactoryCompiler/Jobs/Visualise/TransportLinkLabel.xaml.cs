using System.Linq;
using System.Windows.Controls;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Summary description of a Region in the graph view.
/// </summary>
public partial class TransportLinkLabel : UserControl
{
    public TransportLinkLabel(TransportLinkLabelModel model)
    {
        DataContext = model;
        InitializeComponent();
    }
}

public class TransportLinkLabelModel
{
    private readonly FactoryModel? model;
    private readonly TransportLinkAggregate? linkAggregate;

    public TransportLinkLabelModel()
    {
    }

    public TransportLinkLabelModel(FactoryModel? model, TransportLinkAggregate linkAggregate)
    {
        this.model = model;
        this.linkAggregate = linkAggregate;
    }

    public IItemVolumesState State
    {
        get
        {
            if (model?.Summary == null) return ItemVolumesState.Empty;
            if (linkAggregate == null) return ItemVolumesState.Empty;
            return model.Summary.GetTransportLink(linkAggregate);
        }
    }

    public SurplusAndShortfallModel Summary => new SurplusAndShortfallModel(ItemVolumeSummary.Create(State));
}
