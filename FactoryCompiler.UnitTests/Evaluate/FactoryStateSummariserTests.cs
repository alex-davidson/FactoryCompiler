using System.Collections.Immutable;
using System.Linq;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Evaluate;
using FactoryCompiler.Model.State;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests.Evaluate
{
    [TestFixture]
    public partial class FactoryStateSummariserTests
    {
        [Test]
        public void RegionImportExcessIsInternal()
        {
            var region = new Region(
                "Test",
                groups: ImmutableArray.Create(
                    new Group(null,
                        new Production("Smelter", "Iron Ingot", 4))),
                inbound: ImmutableArray.Create(
                    new Transport("Iron Ingot", "Other")),
                outbound: ImmutableArray<Transport>.Empty);
            var summary = Summarise(new FactoryDescription(ImmutableArray.Create(region)));

            Assert.That(summary.GetNetwork("Other").GetNetVolume(new Item("Iron Ingot")), Is.EqualTo(0));
            Assert.That(summary.GetTransportLink(new TransportLinkAggregate(TransportLinkDirection.FromRegion, region, "Other")).GetNetVolume(new Item("Iron Ingot")), Is.EqualTo(0));
            Assert.That(summary.GetTransportLink(new TransportLinkAggregate(TransportLinkDirection.ToRegion, region, "Other")).GetNetVolume(new Item("Iron Ingot")), Is.EqualTo(0));

            Assert.That(summary.GetRegion(region).GetNetVolume(new Item("Iron Ingot")), Is.EqualTo(120));
        }

        [Test]
        public void RegionExportShortfallIsInternal()
        {
            var region = new Region(
                "Test",
                groups: ImmutableArray.Create(
                    new Group(null,
                        new Production("Smelter", "Iron Ingot", 4))),
                inbound: ImmutableArray<Transport>.Empty,
                outbound: ImmutableArray.Create(
                    new Transport("Iron Ore", "Other")));
            var summary = Summarise(new FactoryDescription(ImmutableArray.Create(region)));

            Assert.That(summary.GetNetwork("Other").GetNetVolume(new Item("Iron Ore")), Is.EqualTo(0));
            Assert.That(summary.GetTransportLink(new TransportLinkAggregate(TransportLinkDirection.FromRegion, region, "Other")).GetNetVolume(new Item("Iron Ore")), Is.EqualTo(0));
            Assert.That(summary.GetTransportLink(new TransportLinkAggregate(TransportLinkDirection.ToRegion, region, "Other")).GetNetVolume(new Item("Iron Ore")), Is.EqualTo(0));

            Assert.That(summary.GetRegion(region).GetNetVolume(new Item("Iron Ore")), Is.EqualTo(-120));
        }

        private FactoryStateSummary Summarise(FactoryDescription description)
        {
            var gameData = new DefaultGameData().Build();
            var stateFactory = new FactoryStateFactory(gameData);
            var state = stateFactory.Create(description);
            Assert.That(state.Diagnostics, Is.Empty);
            new FactoryStateEvaluator().UpdateInPlace(state);

            return new FactoryStateSummariser().Summarise(state);
        }
    }
}
