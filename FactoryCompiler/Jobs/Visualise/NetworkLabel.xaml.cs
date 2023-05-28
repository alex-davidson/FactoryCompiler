using System.Windows.Controls;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Summary description of a Network in the graph view.
/// </summary>
public partial class NetworkLabel : UserControl
{
    public NetworkLabel(NetworkLabelModel model)
    {
        DataContext = model;
        InitializeComponent();
    }
}

public class NetworkLabelModel
{
    private readonly FactoryModel? model;
    private readonly Identifier? network;

    public NetworkLabelModel()
    {
    }

    public NetworkLabelModel(FactoryModel? model, Identifier network)
    {
        this.model = model;
        this.network = network;
    }

    public string Name => network?.Name ?? "";
    public IItemVolumesState State
    {
        get
        {
            if (model?.Summary == null) return ItemVolumesState.Empty;
            if (network == null) return ItemVolumesState.Empty;
            return model.Summary.GetNetwork(network.Value);
        }
    }

    public SurplusAndShortfallModel Summary => new SurplusAndShortfallModel(ItemVolumeSummary.Create(State));
}
