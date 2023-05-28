using System.Collections.Generic;
using System.Linq;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.Model.Evaluate;

public class GroupSummary
{
    public GroupSummary(IItemVolumesState parentContribution, IItemVolumesState groupContribution)
    {
        ParentContribution = parentContribution;
        GroupContribution = groupContribution;
    }

    public IItemVolumesState ParentContribution { get; }
    public IItemVolumesState GroupContribution { get; init; }

    public IItemVolumesState GetScaledContribution() =>
        ItemVolumesState.Sum(ScaleByParentTurnover(ParentContribution, GroupContribution));

    public ItemVolumeSummary GetScaledSummary() =>
        ItemVolumeSummary.Create(GetScaledContribution(), GroupContribution.Select(x => x.Turnover).Sum());

    private IEnumerable<ItemVolume> ScaleByParentTurnover(IItemVolumesState parentItemVolumes, IEnumerable<ItemVolume> itemVolumes)
    {
        foreach (var itemVolume in itemVolumes)
        {
            if (!parentItemVolumes.TryGetDetails(itemVolume.Item, out var details)) continue;   // ??! Should not happen.
            if (details.Volume == 0) continue;  // No net gain/loss. Ignore.

            if (itemVolume.Volume < 0)
            {
                if (details.Volume > 0) continue;   // Parent has excess. No shortfall to report.
                if (details.Produced == 0 || details.Consumed == 0) yield return itemVolume;
                // Allocate proportion of production.
                var fraction = details.Produced / details.Consumed;
                var scaled = fraction * itemVolume.Volume;
                yield return new ItemVolume(itemVolume.Item, scaled);
            }
            else
            {
                if (details.Volume < 0) continue;   // Parent has shortfall. No excess to report.
                if (details.Produced == 0 || details.Consumed == 0) yield return itemVolume;
                // Allocate proportion of consumption.
                var fraction = details.Consumed / details.Produced;
                var scaled = fraction * itemVolume.Volume;
                yield return new ItemVolume(itemVolume.Item, scaled);
            }
        }
    }
}
