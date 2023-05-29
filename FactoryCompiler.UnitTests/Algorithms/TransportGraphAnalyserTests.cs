using System.Collections.Immutable;
using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Algorithms;
using FactoryCompiler.Model.State;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.Algorithms
{
    [TestFixture]
    public class TransportGraphAnalyserTests
    {
        [Test]
        public void CanIdentifyCycle()
        {
            var a = CreateRegion("A");
            var b = CreateRegion("B");
            var links = new []
            {
                new TransportLink(TransportLinkDirection.FromRegion, a, "X", new Item("Item")),
                new TransportLink(TransportLinkDirection.ToRegion, b, "X", new Item("Item")),
                new TransportLink(TransportLinkDirection.FromRegion, b, "Y", new Item("Item")),
                new TransportLink(TransportLinkDirection.ToRegion, a, "Y", new Item("Item")),
            };
            var cycles = new TransportGraphAnalyser().GetCycles(links);
            Assert.That(cycles.Count, Is.EqualTo(1));
            Assert.That(cycles.Single(),
                Is.EquivalentTo(new []
                {
                    new TransportGraphAnalyser.Edge(a, b, "X"),
                    new TransportGraphAnalyser.Edge(b, a, "Y"),
                }));
        }

        private Region CreateRegion(string name) => new Region(name, ImmutableArray<Group>.Empty, ImmutableArray<Transport>.Empty, ImmutableArray<Transport>.Empty);
    }
}
