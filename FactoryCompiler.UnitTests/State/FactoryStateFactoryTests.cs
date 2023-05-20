using FactoryCompiler.Model;
using FactoryCompiler.Model.State;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.State;

[TestFixture]
public class FactoryStateFactoryTests
{
    [Test]
    public void CanCreateFromExampleSimpleFactoryWithoutFailures()
    {
        var gameData = new DefaultGameData().Build();
        var sut = new FactoryStateFactory(gameData);
        var state = sut.Create(Examples.SimpleFactory);
        Assert.That(state.Diagnostics, Is.Empty);
    }
}
