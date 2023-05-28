using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FactoryCompiler.Jobs.Visualise;

public class PieSlice : Shape
{
    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(nameof(Radius), typeof(float), typeof(PieSlice),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// The radius of the pie.
    /// </summary>
    public float Radius
    {
        get => (float)GetValue(RadiusProperty);
        set
        {
            SetValue(RadiusProperty, value);
            Width = value * 2;
            Height = value * 2;
        }
    }

    public static readonly DependencyProperty WedgeAngleProperty =
        DependencyProperty.Register(nameof(WedgeAngle), typeof(float), typeof(PieSlice),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// The wedge angle of this piece in degrees.
    /// </summary>
    public float WedgeAngle
    {
        get => (float)GetValue(WedgeAngleProperty);
        set => SetValue(WedgeAngleProperty, value);
    }

    public static readonly DependencyProperty RotationAngleProperty =
        DependencyProperty.Register(nameof(RotationAngle), typeof(float), typeof(PieSlice),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// The rotation of this piece, in degrees, from the zero point (vertical).
    /// </summary>
    public float RotationAngle
    {
        get => (float)GetValue(RotationAngleProperty);
        set => SetValue(RotationAngleProperty, value);
    }

    protected override Geometry DefiningGeometry
    {
        get
        {
            var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };
            using (var context = geometry.Open())
            {
                DrawGeometry(context);
            }
            geometry.Freeze();
            return geometry;
        }
    }

    private void DrawGeometry(StreamGeometryContext context)
    {
        if (Radius <= 0 || WedgeAngle <= 0) return;

        var centre = new Point(Radius, Radius);
        var arcStartPoint = ComputeCoordinate(RotationAngle, Radius);
        arcStartPoint.Offset(Radius, Radius);
        var arcEndPoint = ComputeCoordinate(RotationAngle + WedgeAngle, Radius);
        arcEndPoint.Offset(Radius, Radius);
        var arcMidPoint = ComputeCoordinate(RotationAngle + WedgeAngle * .5, Radius);
        arcMidPoint.Offset(Radius, Radius);

        var isLargeArc = WedgeAngle > 180.0d;
        // For >180 degrees, need to draw two <180-degree arcs.
        var requiresMidPoint = Math.Abs(WedgeAngle - 360) < .01;

        var outerArcSize = new Size(Radius, Radius);

        if (requiresMidPoint)
        {
            context.BeginFigure(centre, true, true);
            context.LineTo(arcStartPoint, true, true);
            context.ArcTo(arcMidPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
            context.ArcTo(arcEndPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
            context.LineTo(centre, true, true);
            return;
        }

        context.BeginFigure(centre, true, true);
        context.LineTo(arcStartPoint, true, true);
        context.ArcTo(arcEndPoint, outerArcSize, 0, isLargeArc, SweepDirection.Clockwise, true, true);
        context.LineTo(centre, true, true);
    }

    private static Point ComputeCoordinate(double angle, double radius)
    {
        // Convert to radians.
        var angleRad = (Math.PI / 180.0) * (angle - 90);
        // Offset from centre.
        var x = radius * Math.Cos(angleRad);
        var y = radius * Math.Sin(angleRad);

        return new Point(x, y);
    }
}
