using System;
using System.Numerics;

namespace Tsonic.CSharp.Runtime;

internal static class NativeNumbers
{
    private enum Kind { Missing, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, Half, NativeInt, NativeUint, Int128, UInt128 }

    internal static bool IsNumber(object? value) => Classify(value) != Kind.Missing;

    internal static bool TryConvert<T>(object? value, out T result)
    {
        if (!IsNumber(value))
        {
            result = default!;
            return false;
        }
        if (typeof(T) == typeof(sbyte))
        {
            result = (T)(object)Read<sbyte>(value);
            return true;
        }
        if (typeof(T) == typeof(byte))
        {
            result = (T)(object)Read<byte>(value);
            return true;
        }
        if (typeof(T) == typeof(short))
        {
            result = (T)(object)Read<short>(value);
            return true;
        }
        if (typeof(T) == typeof(ushort))
        {
            result = (T)(object)Read<ushort>(value);
            return true;
        }
        if (typeof(T) == typeof(int))
        {
            result = (T)(object)Read<int>(value);
            return true;
        }
        if (typeof(T) == typeof(uint))
        {
            result = (T)(object)Read<uint>(value);
            return true;
        }
        if (typeof(T) == typeof(long))
        {
            result = (T)(object)Read<long>(value);
            return true;
        }
        if (typeof(T) == typeof(ulong))
        {
            result = (T)(object)Read<ulong>(value);
            return true;
        }
        if (typeof(T) == typeof(nint))
        {
            result = (T)(object)Read<nint>(value);
            return true;
        }
        if (typeof(T) == typeof(nuint))
        {
            result = (T)(object)Read<nuint>(value);
            return true;
        }
        if (typeof(T) == typeof(Int128))
        {
            result = (T)(object)Read<Int128>(value);
            return true;
        }
        if (typeof(T) == typeof(UInt128))
        {
            result = (T)(object)Read<UInt128>(value);
            return true;
        }
        if (typeof(T) == typeof(Half))
        {
            result = (T)(object)Read<Half>(value);
            return true;
        }
        if (typeof(T) == typeof(float))
        {
            result = (T)(object)Read<float>(value);
            return true;
        }
        if (typeof(T) == typeof(double))
        {
            result = (T)(object)Read<double>(value);
            return true;
        }
        if (typeof(T) == typeof(decimal))
        {
            result = (T)(object)Read<decimal>(value);
            return true;
        }
        result = default!;
        return false;
    }

    internal static object Binary(object? left, string operation, object? right) => Promote(left, right) switch
    {
        Kind.Int32 => Calculate<int>(left, operation, right),
        Kind.UInt32 => Calculate<uint>(left, operation, right),
        Kind.Int64 => Calculate<long>(left, operation, right),
        Kind.UInt64 => Calculate<ulong>(left, operation, right),
        Kind.Single => Calculate<float>(left, operation, right),
        Kind.Double => Calculate<double>(left, operation, right),
        Kind.Decimal => Calculate<decimal>(left, operation, right),
        Kind.Half => Calculate<Half>(left, operation, right),
        Kind.NativeInt => Calculate<nint>(left, operation, right),
        Kind.NativeUint => Calculate<nuint>(left, operation, right),
        Kind.Int128 => Calculate<Int128>(left, operation, right),
        Kind.UInt128 => Calculate<UInt128>(left, operation, right),
        _ => throw Unsupported(),
    };

    internal static int? Compare(object? left, object? right) => Promote(left, right) switch
    {
        Kind.Int32 => Compare<int>(left, right),
        Kind.UInt32 => Compare<uint>(left, right),
        Kind.Int64 => Compare<long>(left, right),
        Kind.UInt64 => Compare<ulong>(left, right),
        Kind.Single => Compare<float>(left, right),
        Kind.Double => Compare<double>(left, right),
        Kind.Decimal => Compare<decimal>(left, right),
        Kind.Half => Compare<Half>(left, right),
        Kind.NativeInt => Compare<nint>(left, right),
        Kind.NativeUint => Compare<nuint>(left, right),
        Kind.Int128 => Compare<Int128>(left, right),
        Kind.UInt128 => Compare<UInt128>(left, right),
        _ => throw Unsupported(),
    };

    internal static object Unary(object? value, string operation) => (operation, value) switch
    {
        ("+", sbyte number) => +number, ("+", byte number) => +number,
        ("+", short number) => +number, ("+", ushort number) => +number,
        ("+", _) when IsNumber(value) => value!,
        ("-", sbyte number) => -number, ("-", byte number) => -number,
        ("-", short number) => -number, ("-", ushort number) => -number,
        ("-", int number) => -number, ("-", uint number) => -number,
        ("-", long number) => -number, ("-", float number) => -number,
        ("-", double number) => -number, ("-", decimal number) => -number,
        ("-", Half number) => -number, ("-", nint number) => -number,
        ("-", Int128 number) => -number, ("-", UInt128 number) => -number,
        ("~", sbyte number) => ~number, ("~", byte number) => ~number,
        ("~", short number) => ~number, ("~", ushort number) => ~number,
        ("~", int number) => ~number, ("~", uint number) => ~number,
        ("~", long number) => ~number, ("~", ulong number) => ~number,
        ("~", nint number) => ~number, ("~", nuint number) => ~number,
        ("~", Int128 number) => ~number, ("~", UInt128 number) => ~number,
        _ => throw Unsupported(),
    };

    private static T Calculate<T>(object? left, string operation, object? right) where T : INumber<T>
    {
        var first = Read<T>(left);
        var second = Read<T>(right);
        return operation switch
        {
            "+" => first + second, "-" => first - second, "*" => first * second,
            "/" => first / second, "%" => first % second,
            _ => throw Unsupported(),
        };
    }

    private static int? Compare<T>(object? left, object? right) where T : INumber<T>
    {
        var first = Read<T>(left);
        var second = Read<T>(right);
        if (T.IsNaN(first) || T.IsNaN(second)) return null;
        return first.CompareTo(second);
    }

    private static T Read<T>(object? value) where T : INumberBase<T> => value switch
    {
        T exact => exact,
        sbyte number => T.CreateChecked(number), byte number => T.CreateChecked(number),
        short number => T.CreateChecked(number), ushort number => T.CreateChecked(number),
        int number => T.CreateChecked(number), uint number => T.CreateChecked(number),
        long number => T.CreateChecked(number), ulong number => T.CreateChecked(number),
        float number => T.CreateChecked(number), double number => T.CreateChecked(number),
        decimal number => T.CreateChecked(number), Half number => T.CreateChecked(number),
        nint number => T.CreateChecked(number), nuint number => T.CreateChecked(number),
        Int128 number => T.CreateChecked(number), UInt128 number => T.CreateChecked(number),
        _ => throw Unsupported(),
    };

    private static Kind Classify(object? value) => value switch
    {
        sbyte or byte or short or ushort or int => Kind.Int32,
        uint => Kind.UInt32, long => Kind.Int64, ulong => Kind.UInt64,
        float => Kind.Single, double => Kind.Double, decimal => Kind.Decimal,
        Half => Kind.Half, nint => Kind.NativeInt, nuint => Kind.NativeUint,
        Int128 => Kind.Int128, UInt128 => Kind.UInt128,
        _ => Kind.Missing,
    };

    private static Kind Promote(object? left, object? right)
    {
        var first = Classify(left);
        var second = Classify(right);
        if (first == Kind.Missing || second == Kind.Missing) return Kind.Missing;
        if (first == second) return first;
        if ((int)first >= (int)Kind.Half || (int)second >= (int)Kind.Half) return Kind.Missing;
        if (first == Kind.Decimal || second == Kind.Decimal)
            return first is Kind.Single or Kind.Double || second is Kind.Single or Kind.Double ? Kind.Missing : Kind.Decimal;
        if (first == Kind.Double || second == Kind.Double) return Kind.Double;
        if (first == Kind.Single || second == Kind.Single) return Kind.Single;
        if (first == Kind.UInt64 || second == Kind.UInt64)
        {
            var other = first == Kind.UInt64 ? right : left;
            return other is byte or ushort or uint ? Kind.UInt64 : Kind.Missing;
        }
        if (first == Kind.Int64 || second == Kind.Int64) return Kind.Int64;
        if (first == Kind.UInt32 || second == Kind.UInt32)
        {
            var other = first == Kind.UInt32 ? right : left;
            return other is sbyte or short or int ? Kind.Int64 : Kind.UInt32;
        }
        return Kind.Int32;
    }

    private static TypeError Unsupported() => new("The closed numeric operands have no selected native C# operation.");
}
