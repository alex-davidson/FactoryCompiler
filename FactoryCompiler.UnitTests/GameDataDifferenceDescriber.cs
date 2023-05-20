using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FactoryCompiler.Model;

namespace FactoryCompiler.UnitTests;

internal class GameDataDifferenceDescriber
{
    public string Left { get; set; } = "Left";
    public string Right { get; set; } = "Right";

    public string Describe(IGameData left, IGameData right)
    {
        var writer = new StringWriter();
        Describe(left, right, writer);
        return writer.ToString();
    }

    public void Describe(IGameData left, IGameData right, TextWriter differences)
    {
        foreach (var item in Join(left.AllItems, right.AllItems, x => x.Identifier))
        {
            if (item.Left.SetEquals(item.Right)) continue;
            differences.WriteLine($"Item {item.Key} differs.");
            if (item.Left.Count != item.Right.Count)
            {
                differences.WriteLine($"* {Left} has {item.Left.Count}, {Right} has {item.Right.Count}");
            }
        }

        foreach (var recipe in Join(left.Recipes, right.Recipes, x => x.RecipeName))
        {
            if (recipe.Left.SetEquals(recipe.Right)) continue;
            differences.WriteLine($"Recipe {recipe.Key} differs.");
            if (recipe.Left.Count != recipe.Right.Count)
            {
                differences.WriteLine($"* {Left} has {recipe.Left.Count}, {Right} has {recipe.Right.Count}");
            }
        }
    }

    private static IEnumerable<(TProp Key, ISet<T> Left, ISet<T> Right)> Join<T, TProp>(IEnumerable<T> left, IEnumerable<T> right, Func<T, TProp> getKey)
    {
        var leftLookup = left.ToLookup(getKey);
        var rightLookup = right.ToLookup(getKey);

        var allKeys = leftLookup.Concat(rightLookup).Select(l => l.Key).Distinct().ToList();
        foreach (var key in allKeys)
        {
            yield return (key, leftLookup[key].ToHashSet(), rightLookup[key].ToHashSet());
        }
    }
}
