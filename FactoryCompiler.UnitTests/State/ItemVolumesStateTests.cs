using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryCompiler.Model;
using FactoryCompiler.Model.State;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.State;

[TestFixture]
public class ItemVolumesStateTests
{
    [Test]
    public void CanProduce()
    {
        var sut = new ItemVolumesState();
        sut.Produce(new Item("Item"), 50);

        Assert.That(sut,
            Is.EquivalentTo(new []
            {
                new ItemVolume(new Item("Item"), 50),
            }));
    }

    [Test]
    public void CanConsume()
    {
        var sut = new ItemVolumesState();
        sut.Consume(new Item("Item"), 50);

        Assert.That(sut,
            Is.EquivalentTo(new[]
            {
                new ItemVolume(new Item("Item"), -50),
            }));
    }

    [Test]
    public void CanSum()
    {
        var sut1 = new ItemVolumesState();
        sut1.Produce(new Item("Item1"), 50);
        sut1.Consume(new Item("Item2"), 20);
        sut1.Produce(new Item("Item3"), 20);
        var sut2 = new ItemVolumesState();
        sut2.Consume(new Item("Item1"), 50);
        sut2.Produce(new Item("Item2"), 30);
        sut2.Consume(new Item("Item3"), 30);

        Assert.That(ItemVolumesState.Sum(new [] { sut1, sut2 }),
            Is.EquivalentTo(new[]
            {
                new ItemVolume(new Item("Item2"), 10),
                new ItemVolume(new Item("Item3"), -10),
            }));
    }
}
