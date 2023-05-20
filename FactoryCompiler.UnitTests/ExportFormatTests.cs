using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FactoryCompiler.Model;
using FactoryCompiler.Model.Diagnostics;
using NUnit.Framework;

namespace FactoryCompiler.UnitTests
{
    [TestFixture]
    public class ExportFormatTests
    {
        [Test]
        public void CanRoundtripDefaultDatabase()
        {
            var gameData = new DefaultGameData().Build();
            var ms = new MemoryStream();
            var serialiser = new GameDataSerialiser();
            serialiser.Serialise(ms, gameData);
            ms.Position = 0;

            var failures = new List<Diagnostic>();
            var result = serialiser.TryDeserialise(ms, out var roundtripped, failures);
            Assert.That(failures, Is.Empty);
            if (!result)
            {
                Assert.Fail("Deserialisation failed.");
                return;
            }

            if (new GameDataEqualityComparer().Equals(roundtripped, gameData)) return;

            var diff = new GameDataDifferenceDescriber
            {
                Left = "Original",
                Right = "Roundtripped",
            };
            var differences = diff.Describe(gameData, roundtripped!);
            Assert.That(differences, Is.Not.Empty, "Instances compare unequal but no differences were found.");
            Assert.Fail(differences);
        }

        [Test]
        public void DeserialisingInvalidRecipeDoesNotThrow() =>
            TestDeserialisingInvalidDoesNotThrow(new Dto.GameData
            {
                Recipes = new []
                {
                    new Dto.GameData.Recipe
                    {
                        Inputs = new [] { new Dto.GameData.ItemVolume() },
                    },
                },
            });

        [Test]
        public void DeserialisingInvalidFactoryDoesNotThrow() =>
            TestDeserialisingInvalidDoesNotThrow(new Dto.GameData
            {
                Factories = new []
                {
                    new Dto.GameData.Factory(),
                },
            });

        private void TestDeserialisingInvalidDoesNotThrow(Dto.GameData invalid)
        {
            var ms = new MemoryStream();
            JsonSerializer.Serialize(ms, invalid, GameDataJsonContext.Default.GameData);
            ms.Position = 0;

            var failures = new List<Diagnostic>();
            var result = new GameDataSerialiser().TryDeserialise(ms, out _, failures);
            Assert.That(result, Is.False);
            Assert.That(failures, Is.Not.Empty);
        }
    }
}
