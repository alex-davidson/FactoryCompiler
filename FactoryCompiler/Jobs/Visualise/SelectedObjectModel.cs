using System;
using System.Windows;
using Microsoft.Msagl.Drawing;

namespace FactoryCompiler.Jobs.Visualise;

public class SelectedObjectModel
{
    public static SelectedObjectModel None { get; } = new SelectedObjectModel();

    public Visibility Visibility => Object == null ? Visibility.Hidden : Visibility.Visible;
    public DrawingObject? Object { get; init; }
    public FormattedItemVolume[] SortedItemVolumes { get; init; } = Array.Empty<FormattedItemVolume>();

    public DescriptionLines Description { get; } = new DescriptionLines();
}
