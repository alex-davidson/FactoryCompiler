using FactoryCompiler.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryCompiler.Model.State;

namespace FactoryCompiler.UnitTests.Evaluate
{
    public partial class FactoryStateSummariserTests
    {
        [Test]
        public void SimpleImportExportWorksProperly()
        {
            // 30 per building.
            var description = new FactoryDescription(
                ImmutableArray.Create(
                    NetworkHelpers.Exporter("A", 3, "Network"),
                    NetworkHelpers.Importer("B", 4, "Network")));

            var summary = Summarise(description);

            var volume = summary.GetNetwork("Network").GetVolume(new Item("Iron Ore"));
            Assert.That(volume.Volume, Is.EqualTo(-30));
            Assert.That(volume.Produced, Is.EqualTo(90));
            Assert.That(volume.Consumed, Is.EqualTo(120));
        }

        [Test]
        public void SingleRegionResourceImportsAreSpreadAcrossNetworkImports()
        {
            // 30 per building.
            var description = new FactoryDescription(
                ImmutableArray.Create(
                    NetworkHelpers.Exporter("A", 3, "Network"),
                    NetworkHelpers.Exporter("B", 3, "Network"),
                    NetworkHelpers.Importer("C", 5, "Network")));

            var summary = Summarise(description);

            var volume = summary.GetNetwork("Network").GetVolume(new Item("Iron Ore"));
            Assert.That(volume.Volume, Is.EqualTo(30));
            Assert.That(volume.Produced, Is.EqualTo(180));
            Assert.That(volume.Consumed, Is.EqualTo(150));
        }

        [Test]
        public void MultiRegionResourceImportsAreSpreadAcrossNetworkImports()
        {
            // 30 per building.
            var description = new FactoryDescription(
                ImmutableArray.Create(
                    NetworkHelpers.Exporter("A", 3, "Network"),
                    NetworkHelpers.Exporter("B", 3, "Network"),
                    NetworkHelpers.Importer("C", 2, "Network"),
                    NetworkHelpers.Importer("D", 4, "Network")));

            var summary = Summarise(description);

            var volume = summary.GetNetwork("Network").GetVolume(new Item("Iron Ore"));
            Assert.That(volume.Volume, Is.EqualTo(0));
            Assert.That(volume.Produced, Is.EqualTo(180));
            Assert.That(volume.Consumed, Is.EqualTo(180));
        }

        [Test]
        public void SingleRegionMultiNetworkResourceImportsAreSpreadAcrossNetworkImports()
        {
            // 30 per building.
            var description = new FactoryDescription(
                ImmutableArray.Create(
                    NetworkHelpers.Exporter("A", 3, "NetworkA", "NetworkB"),
                    NetworkHelpers.Exporter("B", 3, "NetworkB"),
                    NetworkHelpers.Importer("C", 2, "NetworkA", "NetworkB"),
                    NetworkHelpers.Importer("D", 4, "NetworkB")));

            var summary = Summarise(description);

            var volume = ItemVolumesState.Sum(
                new []
                {
                    summary.GetNetwork("NetworkA"),
                    summary.GetNetwork("NetworkB"),
                }).GetVolume(new Item("Iron Ore"));

            Assert.That(volume.Volume, Is.EqualTo(0));
            Assert.That(volume.Produced, Is.EqualTo(180));
            Assert.That(volume.Consumed, Is.EqualTo(180));
        }

        private static class NetworkHelpers
        {
            public static Region Exporter(string name, int count, params string[] networks) =>
                new Region(
                    name,
                    groups: ImmutableArray.Create(
                        new Group(null,
                            new Production("Miner Mk1", "Impure Iron Ore", count))),
                    inbound: ImmutableArray<Transport>.Empty,
                    outbound: networks.Select(x => new Transport("Iron Ore", x)).ToImmutableArray());

            public static Region Importer(string name, int count, params string[] networks) =>
                new Region(
                    name,
                    groups: ImmutableArray.Create(
                        new Group(null,
                            new Production("Smelter", "Iron Ingot", count))),
                    inbound: networks.Select(x => new Transport("Iron Ore", x)).ToImmutableArray(),
                    outbound: ImmutableArray<Transport>.Empty);
        }
    }
}
