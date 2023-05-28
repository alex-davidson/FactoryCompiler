using System.Collections.Generic;
using Rationals;

namespace FactoryCompiler.Model;

internal static class RationalExtensions
{
    public static Rational Sum(this IEnumerable<Rational> values)
    {
        var accumulator = Rational.Zero;
        foreach (var value in values)
        {
            accumulator += value;
        }
        return accumulator.CanonicalForm;
    }

    public static Rational AbsoluteValue(this Rational value) => value * value.Sign;
}
