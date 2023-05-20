using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FactoryCompiler.Model;

namespace FactoryCompiler.UnitTests;

internal class FactoryDescriptionDifferenceDescriber
{
    public string Left { get; set; } = "Left";
    public string Right { get; set; } = "Right";

    public string Describe(FactoryDescription left, FactoryDescription right)
    {
        var writer = new StringWriter();
        Describe(left, right, writer);
        return writer.ToString();
    }

    public void Describe(FactoryDescription left, FactoryDescription right, TextWriter differences)
    {
        foreach (var item in Join(left.Regions, right.Regions, x => x.RegionName))
        {
            if (item.Left.SetEquals(item.Right)) continue;
            differences.WriteLine($"Region {item.Key} differs.");
            if (item.Left.Count == 1 && item.Right.Count == 1)
            {
                Describe(item.Left.Single(), item.Right.Single(), differences);
            }
            else
            {
                differences.WriteLine($"* {Left} has {item.Left.Count}, {Right} has {item.Right.Count}");
            }
        }
    }

    public void Describe(Region left, Region right, TextWriter differences)
    {
        if (!left.Groups.SetEquals(right.Groups))
        {
            differences.WriteLine("* Groups differ.");
        }
        if (!left.Inbound.SetEquals(right.Inbound))
        {
            differences.WriteLine("* Inbound transports differ.");
        }
        if (!left.Outbound.SetEquals(right.Outbound))
        {
            differences.WriteLine("* Inbound transports differ.");
        }
    }

    private static IEnumerable<(TProp Key, ISet<T> Left, ISet<T> Right)> Join<T, TProp>(IEnumerable<T> left, IEnumerable<T> right, Func<T, TProp> getKey, IEqualityComparer<T>? equalityComparer = null)
    {
        var leftLookup = left.ToLookup(getKey);
        var rightLookup = right.ToLookup(getKey);

        var allKeys = leftLookup.Concat(rightLookup).Select(l => l.Key).Distinct().ToList();
        foreach (var key in allKeys)
        {
            yield return (key, leftLookup[key].ToHashSet(equalityComparer), rightLookup[key].ToHashSet(equalityComparer));
        }
    }
}
