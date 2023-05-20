using System.Text.Json.Serialization;

namespace FactoryCompiler.Model;

[JsonSerializable(typeof(Dto.GameData))]
internal partial class GameDataJsonContext : JsonSerializerContext
{
}
