using System;
using System.Numerics;

namespace Tsonic.CSharp.Runtime;

/// <summary>Exact checked conversion into native integer storage.</summary>
public static class IntegerConversions
{
    /// <summary>Preserves integral input exactly or rejects it without truncation.</summary>
    public static TTarget Checked<TSource, TTarget>(TSource value)
        where TSource : INumberBase<TSource>
        where TTarget : IBinaryInteger<TTarget>
    {
        if (!TSource.IsInteger(value))
            throw new OverflowException("Native integer storage requires a finite integral value.");
        return TTarget.CreateChecked(value);
    }

    /// <summary>Preserves absence or performs an exact checked integer conversion.</summary>
    public static TTarget? CheckedNullable<TSource, TTarget>(TSource? value)
        where TSource : struct, INumberBase<TSource>
        where TTarget : struct, IBinaryInteger<TTarget>
        => value.HasValue ? Checked<TSource, TTarget>(value.Value) : null;
}
