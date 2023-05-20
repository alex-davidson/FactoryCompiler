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
    public class FactoryDescriptionSerialiser
    {
        public void Serialise(Stream stream, FactoryDescription description, bool indent = false)
        {
            var dto = default(FactoryDescriptionModelMapper).MapToDto(description);
            DebugValidate(dto);

            var context = GetWriteContext(indent);
            JsonSerializer.Serialize(stream, dto, context.FactoryDescription);
        }

        public bool TryDeserialise(Stream stream, [MaybeNullWhen(returnValue: false)] out FactoryDescription description, ICollection<Diagnostic>? diagnostics = null)
        {
            var dto = JsonSerializer.Deserialize(stream, readContext.FactoryDescription);
            if (dto == null)
            {
                diagnostics?.Add(Diagnostic.Error("Stream yielded a null factory description."));
                description = null;
                return false;
            }

            var validator = new FactoryDescriptionModelValidator();
            if (diagnostics != null) validator.Diagnostics = diagnostics;

            var initialCount = validator.Diagnostics.Count;
            validator.Validate(dto);
            if (validator.Diagnostics.Skip(initialCount).Any(f => f.Severity >= Severity.Error))
            {
                description = null;
                return false;
            }

            description = default(FactoryDescriptionModelMapper).MapFromDto(dto);
            return true;
        }

        private static FactoryDescriptionJsonContext GetWriteContext(bool indent)
        {
            if (!indent) return FactoryDescriptionJsonContext.Default;
            return new FactoryDescriptionJsonContext(new JsonSerializerOptions { WriteIndented = indent });
        }

        private static readonly FactoryDescriptionJsonContext readContext = new FactoryDescriptionJsonContext(new JsonSerializerOptions { AllowTrailingCommas = true, ReadCommentHandling = JsonCommentHandling.Skip });

        [Conditional("DEBUG")]
        private static void DebugValidate(Dto.FactoryDescription dto)
        {
            var validator = new FactoryDescriptionModelValidator();
            validator.Validate(dto);
            if (validator.Diagnostics.Any()) throw new Exception("DEBUG: generated invalid DTO model from FactoryDescription.");
        }
    }
}
