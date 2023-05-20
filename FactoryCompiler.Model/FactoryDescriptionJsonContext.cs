using System.Text.Json.Serialization;

namespace FactoryCompiler.Model;

[JsonSerializable(typeof(Dto.FactoryDescription))]
internal partial class FactoryDescriptionJsonContext : JsonSerializerContext
{
}
