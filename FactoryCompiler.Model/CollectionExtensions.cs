using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FactoryCompiler.Model;

internal static class CollectionExtensions
{
    public static bool SetEquals<T>(this ImmutableArray<T> a, ImmutableArray<T> b) =>
        !a.Except(b).Any() && !b.Except(a).Any();
    public static bool SetEquals<T>(this ImmutableArray<T> a, ImmutableArray<T> b, IEqualityComparer<T> equalityComparer) =>
        !a.Except(b, equalityComparer).Any() && !b.Except(a, equalityComparer).Any();
}
