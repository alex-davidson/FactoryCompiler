using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Rationals;

namespace FactoryCompiler.Jobs.Visualise;

public readonly struct FormattedItemVolume
{
    public string Item { get; }
    public Rational Volume { get; }
    public Brush Colour { get; }
    public FontWeight FontWeight { get; }
    public string VolumeInteger { get; }
    public string VolumeFraction { get; }

    public FormattedItemVolume(string item, Rational volume, ItemVolumeColourScheme colourScheme, bool highlight)
    {
        Item = item;
        Volume = volume;
        Colour = colourScheme.For(volume);
        FontWeight = highlight ? FontWeights.Bold : FontWeights.Normal;
        VolumeInteger = ((decimal)volume).ToString("#", CultureInfo.CurrentCulture);
        var fraction = decimal.Remainder((decimal)volume, 1).ToString("#.####", CultureInfo.CurrentCulture);
        var decimalSeparator = fraction.IndexOf(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, StringComparison.CurrentCulture);
        VolumeFraction = decimalSeparator < 0 ? "" : fraction[decimalSeparator..];
    }
}
