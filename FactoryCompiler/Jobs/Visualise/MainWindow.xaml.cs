using Microsoft.Msagl.WpfGraphControl;
using System.Windows;
using Microsoft.Msagl.Drawing;

namespace FactoryCompiler.Jobs.Visualise;

public partial class MainWindow : Window
{
    private GraphViewer? graphViewer;
    private readonly VisualiseFactoryModel model;

    public MainWindow() : this(new VisualiseFactoryModel(VisualiseFactoryModelInputs.None))
    {
    }

    public MainWindow(VisualiseFactoryModel model)
    {
        InitializeComponent();

        this.model = model;
        DataContext = model;

        model.PropertyChanged += Model_PropertyChanged;

        SetGraph(model.Graph);
    }

    private void GraphViewer_MouseDown(object? sender, MsaglMouseEventArgs e)
    {
        var obj = graphViewer?.ObjectUnderMouseCursor?.DrawingObject;
        if (obj == null) return;
        model.SetSelectedObject(obj);
    }

    private bool IsZeroArea(Size size) => size.Width <= 0 || size.Height <= 0;

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (IsZeroArea(sizeInfo.PreviousSize) && !IsZeroArea(sizeInfo.NewSize))
        {
            // Initial resize; now the window has a size, lay out the graph.
            ResetLayout();
        }
        base.OnRenderSizeChanged(sizeInfo);
    }

    private void ResetLayout()
    {
        graphViewer?.SetInitialTransform();
    }

    private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(VisualiseFactoryModel.Graph))
        {
            Dispatcher.Invoke(() => SetGraph(model.Graph));
        }
    }

    private void SetGraph(Graph? graph)
    {
        graphViewerPanel.Children.Clear();
        if (graphViewer != null) graphViewer.MouseDown -= GraphViewer_MouseDown;
        graphViewer = null;

        if (graph == null) return;

        graphViewer = new GraphViewer { RunLayoutAsync = true };
        graphViewer.MouseDown += GraphViewer_MouseDown;

        var factoryNodeRenderer = new FactoryNodeRenderer(model.Factory, obj => model.SetSelectedObject(obj));
        foreach (var edge in graph.Edges)
        {
            graphViewer.RegisterLabelCreator(edge, factoryNodeRenderer.Render);
        }
        foreach (var node in graph.Nodes)
        {
            graphViewer.RegisterLabelCreator(node, factoryNodeRenderer.Render);
        }
        foreach (var subgraph in graph.SubgraphMap.Values)
        {
            graphViewer.RegisterLabelCreator(subgraph, factoryNodeRenderer.Render);
        }
        graphViewer.LayoutEditingEnabled = false;
        graphViewer.BindToPanel(graphViewerPanel);

        // This will update the UI.
        graphViewer.Graph = model.Graph;

        // ?
    }
}
