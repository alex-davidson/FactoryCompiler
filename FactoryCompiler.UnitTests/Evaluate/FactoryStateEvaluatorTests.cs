using FactoryCompiler.Model.State;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using NUnit.Framework;
using System.Threading.Tasks;
using Rationals;

namespace FactoryCompiler.UnitTests.Evaluate;

[TestFixture]
public class FactoryStateEvaluatorTests
{
    [Test]
    public void CanEvaluateExampleSimpleFactoryCorrectly()
    {
        TestFactory(Examples.SimpleFactory,
            new ItemVolume(new Item("Iron Ore"), -120),
            new ItemVolume(new Item("Iron Plate"), 60),
            new ItemVolume(new Item("Iron Rod"), 30),
            new ItemVolume(new Item("Copper Ore"), -60),
            new ItemVolume(new Item("Copper Ingot"), 30),
            new ItemVolume(new Item("Wire"), 60));
    }

    [Test]
    public void CanEvaluateExamplePolymerLoopFactoryCorrectly()
    {
        TestFactory(Examples.PolymerLoopFactory,
            new ItemVolume(new Item("Crude Oil"), -300),
            new ItemVolume(new Item("Water"), -1000),
            new ItemVolume(new Item("Plastic"), 500),
            new ItemVolume(new Item("Rubber"), 400));
    }

    [Test]
    public void CanEvaluateExampleAluminumIngotFactoryCorrectly()
    {
        TestFactory(Examples.AluminumIngotFactory,
            new ItemVolume(new Item("Crude Oil"), -60),
            new ItemVolume(new Item("Water"), -180),
            new ItemVolume(new Item("Bauxite"), -600),
            new ItemVolume(new Item("Polymer Resin"), 40),
            new ItemVolume(new Item("Aluminum Ingot"), 600));
    }

    [Test]
    public void CanEvaluateExampleRadioControlUnitFactoryCorrectly()
    {
        TestFactory(Examples.RadioControlUnitFactory,
            new ItemVolume(new Item("Copper Ore"), -60),
            new ItemVolume(new Item("Caterium Ore"), -252),
            new ItemVolume(new Item("Bauxite"), -540),
            new ItemVolume(new Item("Raw Quartz"), -370),
            new ItemVolume(new Item("Coal"), -270),
            new ItemVolume(new Item("Water"), -540),
            new ItemVolume(new Item("Crude Oil"), Rational.Approximate(-461.57, 0.01)),
            new ItemVolume(new Item("Quickwire"), Rational.Approximate(42.86, 0.01)),
            new ItemVolume(new Item("Heavy Oil Residue"), Rational.Approximate(264.86, 0.01)),
            new ItemVolume(new Item("Radio Control Unit"), 18));
    }

    private void TestFactory(FactoryDescription description, params ItemVolume[] expectedItemVolumes)
    {
        var gameData = new DefaultGameData().Build();
        var stateFactory = new FactoryStateFactory(gameData);
        var state = stateFactory.Create(description);
        Assert.That(state.Diagnostics, Is.Empty);
        new FactoryStateEvaluator().UpdateInPlace(state);

        Assert.That(state.ItemVolumes, Is.EquivalentTo(expectedItemVolumes));
    }
}
