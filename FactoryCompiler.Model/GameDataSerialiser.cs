using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using FactoryCompiler.Model.Diagnostics;
using FactoryCompiler.Model.Mappers;

namespace FactoryCompiler.Model
{
    public class GameDataSerialiser
    {
        public void Serialise(Stream stream, IGameData data, bool indent = false)
        {
            var dto = default(GameDataModelMapper).MapToDto(data);
            DebugValidate(dto);

            var context = GetWriteContext(indent);
            JsonSerializer.Serialize(stream, dto, context.GameData);
        }

        public bool TryDeserialise(Stream stream, [MaybeNullWhen(returnValue: false)] out IGameData data, ICollection<Diagnostic>? diagnostics = null)
        {
            var dto = JsonSerializer.Deserialize(stream, readContext.GameData);
            if (dto == null)
            {
                diagnostics?.Add(Diagnostic.Error("Stream yielded a null model."));
                data = null;
                return false;
            }

            var validator = new GameDataModelValidator();
            if (diagnostics != null) validator.Diagnostics = diagnostics;

            var initialCount = validator.Diagnostics.Count;
            validator.Validate(dto);
            if (validator.Diagnostics.Skip(initialCount).Any(f => f.Severity >= Severity.Error))
            {
                data = null;
                return false;
            }

            data = default(GameDataModelMapper).MapFromDto(dto);
            return true;
        }

        private static GameDataJsonContext GetWriteContext(bool indent)
        {
            if (!indent) return GameDataJsonContext.Default;
            return new GameDataJsonContext(new JsonSerializerOptions { WriteIndented = indent });
        }

        private static readonly GameDataJsonContext readContext = new GameDataJsonContext(new JsonSerializerOptions { AllowTrailingCommas = true, ReadCommentHandling = JsonCommentHandling.Skip });

        [Conditional("DEBUG")]
        private static void DebugValidate(Dto.GameData dto)
        {
            var validator = new GameDataModelValidator();
            validator.Validate(dto);
            if (validator.Diagnostics.Any()) throw new Exception("DEBUG: generated invalid DTO model from IGameData.");
        }
    }
}
