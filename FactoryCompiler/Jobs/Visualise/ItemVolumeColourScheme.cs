using System.Windows.Media;
using Rationals;

namespace FactoryCompiler.Jobs.Visualise;

public readonly struct ItemVolumeColourScheme
{
    private static readonly Brush black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

    public static ItemVolumeColourScheme None => new ItemVolumeColourScheme
    {
        Positive = black,
        Negative = black,
        Zero = black,
    };
    public static ItemVolumeColourScheme Area => new ItemVolumeColourScheme
    {
        Positive = new SolidColorBrush(Color.FromRgb(0, 127, 0)),
        Negative = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
        Zero = black,
    };
    public static ItemVolumeColourScheme Transport => new ItemVolumeColourScheme
    {
        Positive = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
        Negative = new SolidColorBrush(Color.FromRgb(230, 200, 0)),
        Zero = black,
    };

    public Brush Positive { get; init; }
    public Brush Negative { get; init; }
    public Brush Zero { get; init; }

    public Brush For(Rational value)
    {
        if (value > 0) return Positive;
        if (value < 0) return Negative;
        return Zero;
    }
}
