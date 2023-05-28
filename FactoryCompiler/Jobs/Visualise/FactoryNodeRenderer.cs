using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FactoryCompiler.Model;
using FactoryCompiler.Model.State;
using Microsoft.Msagl.Drawing;
using Color = System.Windows.Media.Color;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Render a node as a WPF FrameworkElement of some kind.
/// </summary>
internal class FactoryNodeRenderer
{
    private readonly FactoryModel? factoryModel;
    private readonly Action<DrawingObject> onLabelClick;
    private readonly Brush defaultTextBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
    private readonly int defaultFontSize = 12;
    private readonly FontFamily defaultFontFamily = new FontFamily("Arial");

    public FactoryNodeRenderer(FactoryModel? factoryModel, Action<DrawingObject> onLabelClick)
    {
        this.factoryModel = factoryModel;
        this.onLabelClick = onLabelClick;
    }

    public FrameworkElement? Render(DrawingObject obj)
    {
        var element = RenderObject(obj);
        if (element != null)
        {
            element.Tag = obj;
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Width = element.DesiredSize.Width;
            element.Height = element.DesiredSize.Height;
            element.MouseDown += (s, e) => onLabelClick(obj);
        }
        return element;
    }

    public FrameworkElement? RenderObject(DrawingObject obj)
    {
        // obj is a node, subgraph, etc
        switch (obj.UserData)
        {
            case Maybe<Region> { Exists: true, Value: var region }:
                return new RegionLabel(new RegionLabelModel(factoryModel, region));

            case Maybe<GroupState> { Exists: true, Value: var groupState }:
                if (groupState.Production != null)
                {
                    return new ProductionLabel(new ProductionLabelModel(factoryModel, groupState));
                }
                return new GroupLabel(new GroupLabelModel(factoryModel, groupState));

            case Maybe<Identifier> { Exists: true, Value: var network }:
                return new NetworkLabel(new NetworkLabelModel(factoryModel, network));

            case Maybe<TransportLinkAggregate> { Exists: true, Value: var linkAggregate }:
                return new TransportLinkLabel(new TransportLinkLabelModel(factoryModel, linkAggregate));

            default:
                var defaultText = (obj as ILabeledObject)?.Label?.Text;
                if (!string.IsNullOrWhiteSpace(defaultText)) return CreateTextBlock(defaultText);
                return null;
        }
    }

    private TextBlock CreateTextBlock(string text)
    {
        var textBlock = new TextBlock();
        textBlock.Text = text;
        textBlock.FontFamily = defaultFontFamily;
        textBlock.FontSize = defaultFontSize;
        textBlock.Foreground = defaultTextBrush;
        return textBlock;
    }
}
