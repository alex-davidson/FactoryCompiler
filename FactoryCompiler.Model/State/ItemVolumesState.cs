using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rationals;

namespace FactoryCompiler.Model.State;

public class ItemVolumesState : IEnumerable<ItemVolume>
{
    private readonly Dictionary<Item, Rational> volumes;

    public ItemVolumesState() : this(new Dictionary<Item, Rational>())
    {
    }

    private ItemVolumesState(Dictionary<Item, Rational> volumes)
    {
        this.volumes = volumes;
    }

    public void Produce(Item item, Rational volume)
    {
        var existing = GetVolume(item);
        volumes[item] = existing + volume;
    }

    public void Consume(Item item, Rational volume)
    {
        var existing = GetVolume(item);
        volumes[item] = existing - volume;
    }

    public Rational GetVolume(Item item) => volumes.TryGetValue(item, out var existing) ? existing : Rational.Zero;

    public void Reset()
    {
        volumes.Clear();
    }

    public IEnumerator<ItemVolume> GetEnumerator() => volumes.Select(kv => new ItemVolume(kv.Key, kv.Value)).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Sum the specified ItemVolumesStates and remove any items with zero volume (no excess or shortfall).
    /// </summary>
    public static ItemVolumesState Sum(IEnumerable<ItemVolumesState> states) => Sum(states.SelectMany(x => x));

    public static ItemVolumesState Sum(IEnumerable<ItemVolume> itemVolumes)
    {
        var combined = itemVolumes
            .GroupBy(x => x.Item, s => s.Volume, (i, vs) => new ItemVolume(i, Sum(vs)))
            .Where(x => x.Volume != 0)
            .ToDictionary(x => x.Item, x => x.Volume);
        return new ItemVolumesState(combined);
    }

    private static Rational Sum(IEnumerable<Rational> values)
    {
        var accumulator = Rational.Zero;
        foreach (var value in values)
        {
            accumulator += value;
        }
        return accumulator.CanonicalForm;
    }
}
