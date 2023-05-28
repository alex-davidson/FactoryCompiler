using System;
using System.Diagnostics;
using System.Windows;
using FactoryCompiler.Model.Evaluate;

namespace FactoryCompiler.Jobs.Visualise;

public readonly struct SurplusAndShortfallModel
{
    public float Radius { get; }
    public float SurplusFraction { get; }
    public float ShortfallFraction { get; }
    public Visibility Visibility => ShortfallFraction + SurplusFraction > 0 ? Visibility.Visible : Visibility.Hidden;

    public SurplusAndShortfallModel(ItemVolumeSummary summary)
    {
        if (summary.Total == 0)
        {
            this = default;
            return;
        }

        // Area proportional to the logarithm of the total.
        // Aiming for radius between 5 and 20, for between 1 and 20000 items per minute.

        // log(1) = 0
        // log(20000) ~= 4.3
        // sqrt ~= 2
        // 0-2 -> 5-20
        // *7 + 5
        var log10 = Math.Log10((double)summary.Total + 1);
        Radius = (float)(5 + (Math.Sqrt(log10) * 7));

        Debug.Assert(!float.IsNaN(Radius));
        SurplusFraction = (float)(summary.Surplus / summary.Total);
        ShortfallFraction = (float)(summary.Shortfall / summary.Total);
    }
}
