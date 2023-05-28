using System;
using System.Diagnostics.CodeAnalysis;

namespace FactoryCompiler.Jobs.Visualise;

public readonly struct Maybe<T>
{
    private Maybe(T value)
    {
        Exists = value != null;
        Value = value;
    }

    [MemberNotNullWhen(returnValue: true, nameof(Value))]
    public bool Exists { get; }
    public Type Type => typeof(T);
    public T Value { get; }

    public static implicit operator Maybe<T>(T? obj) => obj == null ? default : new Maybe<T>(obj);
}
