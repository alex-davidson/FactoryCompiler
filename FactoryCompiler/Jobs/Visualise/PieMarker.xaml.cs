using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FactoryCompiler.Jobs.Visualise;

/// <summary>
/// Negative/Positive pie chart marker.
/// </summary>
public partial class PieMarker : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty NegativeFractionProperty =
        DependencyProperty.Register(nameof(NegativeFraction),
            typeof(float),
            typeof(PieMarker),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

    public float NegativeFraction
    {
        get => (float)GetValue(NegativeFractionProperty);
        set
        {
            SetValue(NegativeFractionProperty, value);
            OnPropertyChanged(nameof(NegativeWedgeAngle));
            OnPropertyChanged(nameof(PositiveRotationAngle));
        }
    }
    public static readonly DependencyProperty NegativeColourProperty =
        DependencyProperty.Register(nameof(NegativeColour),
            typeof(Brush),
            typeof(PieMarker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public Brush NegativeColour
    {
        get => (Brush)GetValue(NegativeColourProperty);
        set => SetValue(NegativeColourProperty, value);
    }

    public static readonly DependencyProperty PositiveFractionProperty =
        DependencyProperty.Register(nameof(PositiveFraction),
            typeof(float),
            typeof(PieMarker),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

    public float PositiveFraction
    {
        get => (float)GetValue(PositiveFractionProperty);
        set
        {
            SetValue(PositiveFractionProperty, value);
            OnPropertyChanged(nameof(PositiveWedgeAngle));
        }
    }

    public static readonly DependencyProperty PositiveColourProperty =
        DependencyProperty.Register(nameof(PositiveColour),
            typeof(Brush),
            typeof(PieMarker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public Brush PositiveColour
    {
        get => (Brush)GetValue(PositiveColourProperty);
        set => SetValue(PositiveColourProperty, value);
    }

    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(nameof(Radius),
            typeof(float),
            typeof(PieMarker),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

    public float Radius
    {
        get => (float)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public float NegativeWedgeAngle => NegativeFraction * 360;
    public float PositiveRotationAngle => NegativeFraction * 360;
    public float PositiveWedgeAngle => PositiveFraction * 360;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string memberName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
    }

    public PieMarker()
    {
        InitializeComponent();
    }

    protected override Size MeasureOverride(Size constraint)
    {
        return new Size(
            Math.Min(constraint.Width, 2 * Radius),
            Math.Min(constraint.Height, 2 * Radius));
    }
}
