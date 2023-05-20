using System.Globalization;
using Rationals;

namespace FactoryCompiler.Model.Mappers;

internal struct NumberMapper
{
    /// <summary>
    /// Serialise in decimal form if possible to do so without loss of
    /// precision, otherwise canonical rational form.
    /// </summary>
    public string MapToDecimalDto(Rational value)
    {
        var canonical = value.CanonicalForm;
        var decimalForm = (decimal)canonical;
        if ((Rational)decimalForm == canonical)
        {
            return decimalForm.ToString(CultureInfo.InvariantCulture);
        }

        return value.ToString("C");
    }

    /// <summary>
    /// Serialise in percentage form if possible to do so without loss of
    /// precision, otherwise canonical rational form.
    /// </summary>
    public string MapToPercentageDto(Rational value)
    {
        var canonical = value.CanonicalForm;
        var decimalForm = (decimal)canonical;
        if ((Rational)decimalForm == canonical)
        {
            return decimalForm.ToString("0.#####%", CultureInfo.InvariantCulture);
        }

        return value.ToString("C");
    }

    /// <summary>
    /// Deserialise from one of:
    /// * percentage decimal form, if a percentage symbol is detected,
    /// * decimal form, if a decimal point is detected,
    /// * rational '[whole]+[numerator]/[denominator]' form otherwise.
    /// </summary>
    public Rational MapFromDto(string? dto, Rational defaultValue)
    {
        if (dto == null) return defaultValue;
        return MapFromDto(dto);
    }

    /// <summary>
    /// Deserialise from one of:
    /// * percentage decimal form, if a percentage symbol is detected,
    /// * decimal form, if a decimal point is detected,
    /// * rational '[whole]+[numerator]/[denominator]' form otherwise.
    /// </summary>
    public Rational MapFromDto(string dto)
    {
        if (dto.Contains('%'))
        {
            var rawPercentage = decimal.Parse(dto.TrimEnd('%'));
            return ((Rational)rawPercentage) / 100;
        }
        if (dto.Contains('.')) return Rational.ParseDecimal(dto);
        return Rational.Parse(dto);
    }

    /// <summary>
    /// Deserialise from one of:
    /// * decimal form, if a decimal point is detected,
    /// * percentage form, if a percentage symbol is detected,
    /// * rational '[whole]+[numerator]/[denominator]' form otherwise.
    /// </summary>
    public Duration MapDurationFromDto(string dto)
    {
        var rational = MapFromDto(dto);
        return new Duration((decimal)rational);
    }

    public int MapIntegerFromDto(string? dto, int defaultValue)
    {
        if (dto == null) return defaultValue;
        return int.Parse(dto);
    }
}
