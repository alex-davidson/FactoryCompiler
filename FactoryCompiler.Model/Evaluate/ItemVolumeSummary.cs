using System.Linq;
using FactoryCompiler.Model.State;
using Rationals;

namespace FactoryCompiler.Model.Evaluate;

public readonly struct ItemVolumeSummary
{
    public Rational Total { get; }
    public Rational Surplus { get; }
    public Rational Shortfall { get; }

    public ItemVolumeSummary(Rational total, Rational surplus, Rational shortfall)
    {
        Total = total;
        Surplus = surplus;
        Shortfall = shortfall;
    }

    public static ItemVolumeSummary Create(IItemVolumesState itemVolumes, Rational? total = null)
    {
        var surplus = itemVolumes.Select(x => x.Volume).Where(x => x > 0).Sum();
        var shortfall = itemVolumes.Select(x => x.Volume).Where(x => x < 0).Sum();
        return new ItemVolumeSummary(total ?? itemVolumes.Select(x => x.Turnover).Sum(), surplus, -shortfall);
    }

    public override string ToString() => $"+{(Surplus / Total).CanonicalForm}, -{(Shortfall / Total).CanonicalForm}";
}
