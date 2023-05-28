using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rationals;

namespace FactoryCompiler.Model.State;

public interface IItemVolumesState : IEnumerable<ItemVolume>
{
    Rational GetNetVolume(Item item);
    bool TryGetDetails(Item item, out ItemVolume itemVolume);
    ItemVolume GetVolume(Item item);
    IItemVolumesState GetNetVolumes();
}

public class ItemVolumesState : IItemVolumesState
{
    public static IItemVolumesState Empty { get; } = new EmptyItemVolumesState();

    private readonly Dictionary<Item, ItemVolume> volumes;

    public ItemVolumesState() : this(Enumerable.Empty<ItemVolume>())
    {
    }

    private ItemVolumesState(IEnumerable<ItemVolume> volumes)
    {
        this.volumes = volumes.ToDictionary(x => x.Item);
    }

    public void Produce(Item item, Rational volume)
    {
        var existing = GetVolume(item);
        volumes[item] = existing + new ItemVolume(item, volume);
    }

    public void Consume(Item item, Rational volume)
    {
        var existing = GetVolume(item);
        volumes[item] = existing + new ItemVolume(item, -volume);
    }

    public Rational GetNetVolume(Item item) => volumes.TryGetValue(item, out var existing) ? existing.Volume : Rational.Zero;
    public bool TryGetDetails(Item item, out ItemVolume itemVolume) => volumes.TryGetValue(item, out itemVolume);
    public ItemVolume GetVolume(Item item) => volumes.TryGetValue(item, out var existing) ? existing : new ItemVolume(item, 0);

    public void Reset()
    {
        volumes.Clear();
    }

    public IEnumerator<ItemVolume> GetEnumerator() => volumes.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IItemVolumesState GetNetVolumes() =>
        new ItemVolumesState(volumes.Values
            .Where(x => x.Volume != 0)
            .Select(x => new ItemVolume(x.Item, x.Volume)));

    /// <summary>
    /// Sum the specified ItemVolumesStates and remove any items with zero volume (no excess or shortfall).
    /// </summary>
    public static ItemVolumesState Sum(IEnumerable<IItemVolumesState> states) => Sum(states.SelectMany(x => x));

    public static ItemVolumesState Sum(IEnumerable<ItemVolume> itemVolumes)
    {
        var combined = itemVolumes.GroupBy(x => x.Item, SumItemVolume);
        return new ItemVolumesState(combined);
    }

    private static ItemVolume SumItemVolume(Item item, IEnumerable<ItemVolume> volumes)
    {
        var produced = volumes.Select(x => x.Produced).Sum();
        var consumed = volumes.Select(x => x.Consumed).Sum();
        return new ItemVolume(item, produced, consumed);
    }

    private class EmptyItemVolumesState : IItemVolumesState
    {
        public IEnumerator<ItemVolume> GetEnumerator() { yield break; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Rational GetNetVolume(Item item) => 0;
        public ItemVolume GetVolume(Item item) => new ItemVolume(item, 0);
        public IItemVolumesState GetNetVolumes() => this;

        public bool TryGetDetails(Item item, out ItemVolume itemVolume)
        {
            itemVolume = new ItemVolume(item, 0);
            return false;
        }
    }
}
