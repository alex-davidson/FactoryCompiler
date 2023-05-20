using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Text.Json;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;
using FactoryCompiler.Model.Mappers;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests
{
    [TestFixture]
    public class FactoryDescriptionImportTests
    {
        [Test]
        public void CanReadSimpleRegion()
        {
            var description = new FactoryDescription(
                ImmutableArray.Create(
                    new Region(
                        "Region",
                        ImmutableArray.Create(
                            new Group("Iron",
                                ImmutableArray.Create(
                                    new Group(null,
                                        new Production("Constructor", "Iron Plate", 2), 3)))),
                        ImmutableArray.Create(
                            new Transport("Iron Ingot", "Iron mine")
                        ),
                        ImmutableArray.Create(
                            new Transport("Iron Plate", "Everywhere")
                        ))));

            var ms = new MemoryStream();
            var serialiser = new FactoryDescriptionSerialiser();
            serialiser.Serialise(ms, description);
            ms.Position = 0;

            var failures = new List<Diagnostic>();
            var result = serialiser.TryDeserialise(ms, out var roundtripped, failures);
            Assert.That(failures, Is.Empty);
            if (!result)
            {
                Assert.Fail("Deserialisation failed.");
                return;
            }

            if (Equals(roundtripped, description)) return;

            var diff = new FactoryDescriptionDifferenceDescriber
            {
                Left = "Original",
                Right = "Roundtripped",
            };
            var differences = diff.Describe(description, roundtripped!);
            Assert.That(differences, Is.Not.Empty, "Instances compare unequal but no differences were found.");
            Assert.Fail(differences);
        }

        [Test]
        public void DeserialisingInvalidRegionDoesNotThrow() =>
            TestDeserialisingInvalidDoesNotThrow(new Dto.FactoryDescription
            {
                Regions = new []
                {
                    new Dto.FactoryDescription.Region
                    {
                        Groups = new []
                        {
                            new Dto.FactoryDescription.Group
                            {
                                Recipe = "Iron Plate",
                                Groups = new []
                                {
                                    new Dto.FactoryDescription.Group
                                    {
                                        GroupName = "Empty",
                                    },
                                },
                            },
                        },
                    },
                },
            });

        private void TestDeserialisingInvalidDoesNotThrow(Dto.FactoryDescription invalid)
        {
            var ms = new MemoryStream();
            JsonSerializer.Serialize(ms, invalid, FactoryDescriptionJsonContext.Default.FactoryDescription);
            ms.Position = 0;

            var failures = new List<Diagnostic>();
            var result = new FactoryDescriptionSerialiser().TryDeserialise(ms, out _, failures);
            Assert.That(result, Is.False);
            Assert.That(failures, Is.Not.Empty);
        }
    }
}
