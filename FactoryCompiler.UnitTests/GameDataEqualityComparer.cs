using System.Collections.Generic;
using FactoryCompiler.Model;

namespace FactoryCompiler.UnitTests;

internal class GameDataEqualityComparer : IEqualityComparer<IGameData>
{
    public bool Equals(IGameData? x, IGameData? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        return x.BaseItems.SetEquals(y.BaseItems) &&
               x.AllItems.SetEquals(y.AllItems) &&
               x.Factories.SetEquals(y.Factories) &&
               x.Recipes.SetEquals(y.Recipes);
    }

    public int GetHashCode(IGameData obj)
    {
        unchecked
        {
            var hashCode = 0;
            foreach (var item in obj.AllItems)
            {
                hashCode *= 397;
                hashCode ^= item.GetHashCode();
            }
            return hashCode;
        }
    }
}
