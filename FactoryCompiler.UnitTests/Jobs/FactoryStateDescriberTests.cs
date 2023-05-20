using System.IO;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;
using FactoryCompiler.Model;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.Jobs;

[TestFixture]
public class FactoryStateDescriberTests
{
    [Test]
    public void CanSummariseExampleSimpleFactoryCorrectly()
    {
        TestFactory(Examples.SimpleFactory, @"
Region: Factories
  Excess:
              Iron Plate:         60
                    Wire:         60

Region: Smelting
  (balanced)

Network: Ammo Factory
  Excess:
                Iron Rod:         30

Network: Ingots
  Excess:
            Copper Ingot:         30

Network: Mines
  Shortfall:
                Iron Ore:       -120
              Copper Ore:        -60

".TrimStart());
    }

    private void TestFactory(FactoryDescription description, string expectedSummary)
    {
        var gameData = new DefaultGameData().Build();
        var stateFactory = new FactoryStateFactory(gameData);
        var state = stateFactory.Create(description);
        Assert.That(state.Diagnostics, Is.Empty);
        new FactoryStateEvaluator().UpdateInPlace(state);

        var summary = new FactoryStateSummariser().Summarise(state);

        using (var writer = new StringWriter())
        {
            new FactoryStateFormatter(writer).Format(summary);
            Assert.That(writer.ToString(), Is.EquivalentTo(expectedSummary));
        }
    }
}
