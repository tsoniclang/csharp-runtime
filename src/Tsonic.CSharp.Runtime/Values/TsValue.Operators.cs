using System;
using System.Globalization;

namespace Tsonic.CSharp.Runtime;

public readonly partial struct TsValue
{
    public static TsValue Add(object? left, object? right) => plus(left, right);
    public static TsValue Subtract(object? left, object? right) => from(NativeNumbers.Subtract(unwrapForOperation(left), unwrapForOperation(right)));
    public static TsValue Multiply(object? left, object? right) => from(NativeNumbers.Multiply(unwrapForOperation(left), unwrapForOperation(right)));
    public static TsValue Divide(object? left, object? right) => from(NativeNumbers.Divide(unwrapForOperation(left), unwrapForOperation(right)));
    public static TsValue Remainder(object? left, object? right) => from(NativeNumbers.Remainder(unwrapForOperation(left), unwrapForOperation(right)));

    public static bool Equal(TsValue left, TsValue right) => looseEquals(left._value, right._value);
    public static bool Equal(object? left, object? right) => looseEquals(left, right);
    public static bool NotEqual(TsValue left, TsValue right) => !looseEquals(left._value, right._value);
    public static bool NotEqual(object? left, object? right) => !looseEquals(left, right);
    public static bool StrictEqual(TsValue left, TsValue right) => strictEquals(left._value, right._value);
    public static bool StrictEqual(object? left, object? right) => strictEquals(left, right);
    public static bool StrictNotEqual(TsValue left, TsValue right) => !strictEquals(left._value, right._value);
    public static bool StrictNotEqual(object? left, object? right) => !strictEquals(left, right);
    public static bool LessThan(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison < 0;
    public static bool LessThanOrEqual(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison <= 0;
    public static bool GreaterThan(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison > 0;
    public static bool GreaterThanOrEqual(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison >= 0;

    public static TsValue Positive(object? operand) => from(NativeNumbers.Positive(unwrapForOperation(operand)));
    public static TsValue Negate(object? operand) => from(NativeNumbers.Negate(unwrapForOperation(operand)));
    public static TsValue BitwiseNot(object? operand) => from(NativeNumbers.BitwiseNot(unwrapForOperation(operand)));
    public static bool LogicalNot(object? operand) => !truthy(operand);

    public static TsValue Coalesce(object? left, Func<object?> right) => isNullish(left) ? from(right()) : from(left);
    public static TsValue And(object? left, Func<object?> right) => truthy(left) ? from(right()) : from(left);
    public static TsValue Or(object? left, Func<object?> right) => truthy(left) ? from(left) : from(right());

    public static TsValue Add(TsValue left, TsValue right) => Add(left._value, right._value);
    public static TsValue Add(TsValue left, object? right) => Add(left._value, right);
    public static TsValue Add(object? left, TsValue right) => Add(left, right._value);
    public static TsValue Subtract(TsValue left, TsValue right) => Subtract(left._value, right._value);
    public static TsValue Subtract(TsValue left, object? right) => Subtract(left._value, right);
    public static TsValue Subtract(object? left, TsValue right) => Subtract(left, right._value);
    public static TsValue Multiply(TsValue left, TsValue right) => Multiply(left._value, right._value);
    public static TsValue Multiply(TsValue left, object? right) => Multiply(left._value, right);
    public static TsValue Multiply(object? left, TsValue right) => Multiply(left, right._value);
    public static TsValue Divide(TsValue left, TsValue right) => Divide(left._value, right._value);
    public static TsValue Divide(TsValue left, object? right) => Divide(left._value, right);
    public static TsValue Divide(object? left, TsValue right) => Divide(left, right._value);
    public static TsValue Remainder(TsValue left, TsValue right) => Remainder(left._value, right._value);
    public static TsValue Remainder(TsValue left, object? right) => Remainder(left._value, right);
    public static TsValue Remainder(object? left, TsValue right) => Remainder(left, right._value);
    public static bool Equal(TsValue left, object? right) => Equal(left._value, right);
    public static bool Equal(object? left, TsValue right) => Equal(left, right._value);
    public static bool NotEqual(TsValue left, object? right) => NotEqual(left._value, right);
    public static bool NotEqual(object? left, TsValue right) => NotEqual(left, right._value);
    public static bool StrictEqual(TsValue left, object? right) => StrictEqual(left._value, right);
    public static bool StrictEqual(object? left, TsValue right) => StrictEqual(left, right._value);
    public static bool StrictNotEqual(TsValue left, object? right) => StrictNotEqual(left._value, right);
    public static bool StrictNotEqual(object? left, TsValue right) => StrictNotEqual(left, right._value);
    public static bool LessThan(TsValue left, TsValue right) => LessThan(left._value, right._value);
    public static bool LessThan(TsValue left, object? right) => LessThan(left._value, right);
    public static bool LessThan(object? left, TsValue right) => LessThan(left, right._value);
    public static bool LessThanOrEqual(TsValue left, TsValue right) => LessThanOrEqual(left._value, right._value);
    public static bool LessThanOrEqual(TsValue left, object? right) => LessThanOrEqual(left._value, right);
    public static bool LessThanOrEqual(object? left, TsValue right) => LessThanOrEqual(left, right._value);
    public static bool GreaterThan(TsValue left, TsValue right) => GreaterThan(left._value, right._value);
    public static bool GreaterThan(TsValue left, object? right) => GreaterThan(left._value, right);
    public static bool GreaterThan(object? left, TsValue right) => GreaterThan(left, right._value);
    public static bool GreaterThanOrEqual(TsValue left, TsValue right) => GreaterThanOrEqual(left._value, right._value);
    public static bool GreaterThanOrEqual(TsValue left, object? right) => GreaterThanOrEqual(left._value, right);
    public static bool GreaterThanOrEqual(object? left, TsValue right) => GreaterThanOrEqual(left, right._value);
    public static TsValue Positive(TsValue operand) => Positive(operand._value);
    public static TsValue Negate(TsValue operand) => Negate(operand._value);
    public static TsValue BitwiseNot(TsValue operand) => BitwiseNot(operand._value);
    public static bool LogicalNot(TsValue operand) => LogicalNot(operand._value);
    public static TsValue Coalesce(TsValue left, Func<object?> right) => isNullish(left._value) ? from(right()) : left;
    public static TsValue And(TsValue left, Func<object?> right) => truthy(left._value) ? from(right()) : left;
    public static TsValue Or(TsValue left, Func<object?> right) => truthy(left._value) ? left : from(right());
    public static bool ToDynamicBoolean(TsValue value) => truthy(value._value);
    public static TsValue ApplyDynamicVoid(TsValue operand) => undefined();

    private static TsValue plus(object? left, object? right)
    {
        var leftValue = unwrapForOperation(left);
        var rightValue = unwrapForOperation(right);
        return leftValue is string || rightValue is string
            ? from(toJsString(leftValue) + toJsString(rightValue))
            : from(NativeNumbers.Add(leftValue, rightValue));
    }

    private static bool tryCompare(object? left, object? right, out int comparison)
    {
        var leftValue = unwrapForOperation(left);
        var rightValue = unwrapForOperation(right);
        if (leftValue is string leftText && rightValue is string rightText)
        {
            comparison = string.CompareOrdinal(leftText, rightText);
            return true;
        }
        var result = NativeNumbers.Compare(leftValue, rightValue);
        comparison = result.GetValueOrDefault();
        return result.HasValue;
    }

    private static bool looseEquals(object? left, object? right)
    {
        var leftValue = unwrapForOperation(left);
        var rightValue = unwrapForOperation(right);
        if (isNullish(leftValue) && isNullish(rightValue))
        {
            return true;
        }
        if (isNullish(leftValue) || isNullish(rightValue))
        {
            return false;
        }
        return strictEquals(leftValue, rightValue);
    }

    private static bool strictEquals(object? left, object? right)
    {
        var leftValue = unwrapForOperation(left);
        var rightValue = unwrapForOperation(right);
        if (NativeNumbers.IsNumber(leftValue) && NativeNumbers.IsNumber(rightValue))
            return NativeNumbers.Compare(leftValue, rightValue) == 0;
        if (ReferenceEquals(leftValue, rightValue))
        {
            return true;
        }
        if (isNullish(leftValue) || isNullish(rightValue))
        {
            return false;
        }
        return leftValue!.GetType() == rightValue!.GetType() && Equals(leftValue, rightValue);
    }

    private static bool truthy(object? value)
    {
        var unwrapped = unwrapForOperation(value);
        return unwrapped switch
        {
            null => false,
            bool boolean => boolean,
            string text => text.Length > 0,
            double number => number != 0 && !double.IsNaN(number),
            float number => number != 0 && !float.IsNaN(number),
            decimal number => number != 0,
            int number => number != 0,
            long number => number != 0,
            uint number => number != 0,
            ulong number => number != 0,
            byte number => number != 0,
            sbyte number => number != 0,
            short number => number != 0,
            ushort number => number != 0,
            Half number => number != (Half)0 && !Half.IsNaN(number),
            nint number => number != 0,
            nuint number => number != 0,
            Int128 number => number != 0,
            UInt128 number => number != 0,
            _ => true
        };
    }

    private static string toJsString(object? value)
    {
        var unwrapped = unwrapForOperation(value);
        return unwrapped switch
        {
            null => "null",
            bool boolean => boolean ? "true" : "false",
            string text => text,
            double number => number.ToString(CultureInfo.InvariantCulture),
            float number => number.ToString(CultureInfo.InvariantCulture),
            decimal number => number.ToString(CultureInfo.InvariantCulture),
            int number => number.ToString(CultureInfo.InvariantCulture),
            long number => number.ToString(CultureInfo.InvariantCulture),
            uint number => number.ToString(CultureInfo.InvariantCulture),
            ulong number => number.ToString(CultureInfo.InvariantCulture),
            byte number => number.ToString(CultureInfo.InvariantCulture),
            sbyte number => number.ToString(CultureInfo.InvariantCulture),
            short number => number.ToString(CultureInfo.InvariantCulture),
            ushort number => number.ToString(CultureInfo.InvariantCulture),
            Half number => number.ToString(CultureInfo.InvariantCulture),
            nint number => number.ToString(CultureInfo.InvariantCulture),
            nuint number => number.ToString(CultureInfo.InvariantCulture),
            Int128 number => number.ToString(CultureInfo.InvariantCulture),
            UInt128 number => number.ToString(CultureInfo.InvariantCulture),
            _ => "[object Object]"
        };
    }
}
