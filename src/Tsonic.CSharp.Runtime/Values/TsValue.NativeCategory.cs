using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tsonic.CSharp.Runtime;

public readonly partial struct TsValue
{
    public static SourceValueCategory nativecategory<TValue>(TValue value)
    {
        if (typeof(TValue) == typeof(object)) return checkedNativeCategory(value);
        if (typeof(TValue) == typeof(TsValue)) return Unsafe.As<TValue, TsValue>(ref value).Category;
        if (typeof(TValue) == typeof(bool)) return SourceValueCategory.Boolean;
        if (typeof(TValue) == typeof(long) || typeof(TValue) == typeof(ulong) ||
            typeof(TValue) == typeof(Int128) || typeof(TValue) == typeof(UInt128) ||
            typeof(TValue) == typeof(BigInteger)) return SourceValueCategory.BigInt;
        if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(float) ||
            typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(Half) ||
            typeof(TValue) == typeof(int) || typeof(TValue) == typeof(uint) ||
            typeof(TValue) == typeof(byte) || typeof(TValue) == typeof(sbyte) ||
            typeof(TValue) == typeof(short) || typeof(TValue) == typeof(ushort) ||
            typeof(TValue) == typeof(nint) || typeof(TValue) == typeof(nuint)) return SourceValueCategory.Number;
        if (typeof(TValue) == typeof(bool?))
            return value is null ? SourceValueCategory.Absent : SourceValueCategory.Boolean;
        if (typeof(TValue) == typeof(long?) || typeof(TValue) == typeof(ulong?) ||
            typeof(TValue) == typeof(Int128?) || typeof(TValue) == typeof(UInt128?) ||
            typeof(TValue) == typeof(BigInteger?))
            return value is null ? SourceValueCategory.Absent : SourceValueCategory.BigInt;
        if (typeof(TValue) == typeof(double?) || typeof(TValue) == typeof(float?) ||
            typeof(TValue) == typeof(decimal?) || typeof(TValue) == typeof(Half?) ||
            typeof(TValue) == typeof(int?) || typeof(TValue) == typeof(uint?) ||
            typeof(TValue) == typeof(byte?) || typeof(TValue) == typeof(sbyte?) ||
            typeof(TValue) == typeof(short?) || typeof(TValue) == typeof(ushort?) ||
            typeof(TValue) == typeof(nint?) || typeof(TValue) == typeof(nuint?))
            return value is null ? SourceValueCategory.Absent : SourceValueCategory.Number;
        return checkedNativeCategory(value);
    }

    private static SourceValueCategory checkedNativeCategory(object? payload)
    {
        if (payload is TsValue stored) return stored.Category;
        if (payload is IClosedUnionValue union)
        {
            if (!union.IsInitialized) throw new InvalidOperationException("Union is not initialized.");
            return union.NativeCategory;
        }
        validateClosedValue(payload);
        return payload switch
        {
            null => SourceValueCategory.Absent,
            bool => SourceValueCategory.Boolean,
            string => SourceValueCategory.String,
            long or ulong or Int128 or UInt128 or BigInteger => SourceValueCategory.BigInt,
            double or float or decimal or Half or int or uint or byte or sbyte or short or ushort or nint or nuint => SourceValueCategory.Number,
            TsFunction or Delegate => SourceValueCategory.Function,
            _ => SourceValueCategory.Object,
        };
    }
}
