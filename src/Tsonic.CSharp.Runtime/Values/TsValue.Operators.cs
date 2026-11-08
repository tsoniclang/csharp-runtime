using System;
using System.Globalization;

namespace Tsonic.CSharp.Runtime;

public readonly partial struct TsValue
{
    public static TsValue Add(object? left, object? right) => plus(left, right, valueCategory(left), valueCategory(right));
    public static TsValue Subtract(object? left, object? right) => numericResult(NativeNumbers.Subtract(unwrapForOperation(left), unwrapForOperation(right)), valueCategory(left), valueCategory(right));
    public static TsValue Multiply(object? left, object? right) => numericResult(NativeNumbers.Multiply(unwrapForOperation(left), unwrapForOperation(right)), valueCategory(left), valueCategory(right));
    public static TsValue Divide(object? left, object? right) => numericResult(NativeNumbers.Divide(unwrapForOperation(left), unwrapForOperation(right)), valueCategory(left), valueCategory(right));
    public static TsValue Remainder(object? left, object? right) => numericResult(NativeNumbers.Remainder(unwrapForOperation(left), unwrapForOperation(right)), valueCategory(left), valueCategory(right));

    public static bool Equal(TsValue left, TsValue right) => looseEquals(left._value, right._value);
    public static bool Equal(object? left, object? right) => looseEquals(left, right);
    public static bool NotEqual(TsValue left, TsValue right) => !looseEquals(left._value, right._value);
    public static bool NotEqual(object? left, object? right) => !looseEquals(left, right);
    public static bool StrictEqual(TsValue left, TsValue right) => strictCategoryEquals(left._value, left.Category, right._value, right.Category);
    public static bool StrictEqual(object? left, object? right) => strictCategoryEquals(UnwrapDynamicCarrier(left), valueCategory(left), UnwrapDynamicCarrier(right), valueCategory(right));
    public static bool StrictNotEqual(TsValue left, TsValue right) => !StrictEqual(left, right);
    public static bool StrictNotEqual(object? left, object? right) => !StrictEqual(left, right);
    public static bool LessThan(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison < 0;
    public static bool LessThanOrEqual(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison <= 0;
    public static bool GreaterThan(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison > 0;
    public static bool GreaterThanOrEqual(object? left, object? right) => tryCompare(left, right, out var comparison) && comparison >= 0;

    public static TsValue Positive(object? operand) => numericResult(NativeNumbers.Positive(unwrapForOperation(operand)), valueCategory(operand), valueCategory(operand));
    public static TsValue Negate(object? operand) => numericResult(NativeNumbers.Negate(unwrapForOperation(operand)), valueCategory(operand), valueCategory(operand));
    public static TsValue BitwiseNot(object? operand) => numericResult(NativeNumbers.BitwiseNot(unwrapForOperation(operand)), valueCategory(operand), valueCategory(operand));
    public static bool LogicalNot(object? operand) => !truthy(operand);

    public static TsValue Coalesce(object? left, Func<object?> right) => isNullish(left) ? fromnative(right()) : fromnative(left);
    public static TsValue And(object? left, Func<object?> right) => truthy(left) ? fromnative(right()) : fromnative(left);
    public static TsValue Or(object? left, Func<object?> right) => truthy(left) ? fromnative(left) : fromnative(right());

    public static TsValue Add(TsValue left, TsValue right) => plus(left._value, right._value, left.Category, right.Category);
    public static TsValue Add(TsValue left, object? right) => plus(left._value, right, left.Category, valueCategory(right));
    public static TsValue Add(object? left, TsValue right) => plus(left, right._value, valueCategory(left), right.Category);
    public static TsValue Subtract(TsValue left, TsValue right) => numericResult(NativeNumbers.Subtract(unwrapForOperation(left._value), unwrapForOperation(right._value)), left.Category, right.Category);
    public static TsValue Subtract(TsValue left, object? right) => numericResult(NativeNumbers.Subtract(unwrapForOperation(left._value), unwrapForOperation(right)), left.Category, valueCategory(right));
    public static TsValue Subtract(object? left, TsValue right) => numericResult(NativeNumbers.Subtract(unwrapForOperation(left), unwrapForOperation(right._value)), valueCategory(left), right.Category);
    public static TsValue Multiply(TsValue left, TsValue right) => numericResult(NativeNumbers.Multiply(unwrapForOperation(left._value), unwrapForOperation(right._value)), left.Category, right.Category);
    public static TsValue Multiply(TsValue left, object? right) => numericResult(NativeNumbers.Multiply(unwrapForOperation(left._value), unwrapForOperation(right)), left.Category, valueCategory(right));
    public static TsValue Multiply(object? left, TsValue right) => numericResult(NativeNumbers.Multiply(unwrapForOperation(left), unwrapForOperation(right._value)), valueCategory(left), right.Category);
    public static TsValue Divide(TsValue left, TsValue right) => numericResult(NativeNumbers.Divide(unwrapForOperation(left._value), unwrapForOperation(right._value)), left.Category, right.Category);
    public static TsValue Divide(TsValue left, object? right) => numericResult(NativeNumbers.Divide(unwrapForOperation(left._value), unwrapForOperation(right)), left.Category, valueCategory(right));
    public static TsValue Divide(object? left, TsValue right) => numericResult(NativeNumbers.Divide(unwrapForOperation(left), unwrapForOperation(right._value)), valueCategory(left), right.Category);
    public static TsValue Remainder(TsValue left, TsValue right) => numericResult(NativeNumbers.Remainder(unwrapForOperation(left._value), unwrapForOperation(right._value)), left.Category, right.Category);
    public static TsValue Remainder(TsValue left, object? right) => numericResult(NativeNumbers.Remainder(unwrapForOperation(left._value), unwrapForOperation(right)), left.Category, valueCategory(right));
    public static TsValue Remainder(object? left, TsValue right) => numericResult(NativeNumbers.Remainder(unwrapForOperation(left), unwrapForOperation(right._value)), valueCategory(left), right.Category);
    public static bool Equal(TsValue left, object? right) => Equal(left._value, right);
    public static bool Equal(object? left, TsValue right) => Equal(left, right._value);
    public static bool NotEqual(TsValue left, object? right) => NotEqual(left._value, right);
    public static bool NotEqual(object? left, TsValue right) => NotEqual(left, right._value);
    public static bool StrictEqual(TsValue left, object? right) => strictCategoryEquals(left._value, left.Category, UnwrapDynamicCarrier(right), valueCategory(right));
    public static bool StrictEqual(object? left, TsValue right) => strictCategoryEquals(UnwrapDynamicCarrier(left), valueCategory(left), right._value, right.Category);
    public static bool StrictNotEqual(TsValue left, object? right) => !StrictEqual(left, right);
    public static bool StrictNotEqual(object? left, TsValue right) => !StrictEqual(left, right);
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
    public static TsValue Positive(TsValue operand) => numericResult(NativeNumbers.Positive(unwrapForOperation(operand._value)), operand.Category, operand.Category);
    public static TsValue Negate(TsValue operand) => numericResult(NativeNumbers.Negate(unwrapForOperation(operand._value)), operand.Category, operand.Category);
    public static TsValue BitwiseNot(TsValue operand) => numericResult(NativeNumbers.BitwiseNot(unwrapForOperation(operand._value)), operand.Category, operand.Category);
    public static bool LogicalNot(TsValue operand) => !truthy(operand._value, operand.Category);
    public static TsValue Coalesce(TsValue left, Func<object?> right) => left.isUndefined() ? fromnative(right()) : left;
    public static TsValue And(TsValue left, Func<object?> right) => truthy(left._value, left.Category) ? fromnative(right()) : left;
    public static TsValue Or(TsValue left, Func<object?> right) => truthy(left._value, left.Category) ? left : fromnative(right());
    public static bool ToDynamicBoolean(TsValue value) => truthy(value._value, value.Category);
    public static TsValue ApplyDynamicVoid(TsValue operand) => undefined();

    private static TsValue plus(object? left, object? right, SourceValueCategory leftCategory, SourceValueCategory rightCategory)
    {
        var leftValue = unwrapForOperation(left);
        var rightValue = unwrapForOperation(right);
        return leftValue is string || rightValue is string
            ? fromnative(toJsString(leftValue) + toJsString(rightValue))
            : numericResult(NativeNumbers.Add(leftValue, rightValue), leftCategory, rightCategory);
    }

    private static TsValue numericResult(object? value, SourceValueCategory left, SourceValueCategory right)
    {
        var category = left == SourceValueCategory.Number || right == SourceValueCategory.Number
            ? SourceValueCategory.Number : SourceValueCategory.BigInt;
        return fromsource(value, category);
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
        if (leftValue is Delegate || rightValue is Delegate)
        {
            return false;
        }
        return leftValue!.GetType() == rightValue!.GetType() && Equals(leftValue, rightValue);
    }

    private static bool strictCategoryEquals(object? left, SourceValueCategory leftCategory, object? right, SourceValueCategory rightCategory)
    {
        if (leftCategory != rightCategory) return false;
        if (leftCategory == SourceValueCategory.Absent) return true;
        if (leftCategory is SourceValueCategory.Object or SourceValueCategory.Function or SourceValueCategory.Symbol)
            return ReferenceEquals(left, right) || left is not null && left.GetType().IsValueType &&
                left.GetType() == right?.GetType() && left.Equals(right);
        return strictEquals(left, right);
    }

    private static bool truthy(object? value)
    {
        if (value is TsValue stored) return truthy(stored._value, stored.Category);
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
            System.Numerics.BigInteger number => !number.IsZero,
            _ => true
        };
    }

    private static bool truthy(object? value, SourceValueCategory category) => category switch
    {
        SourceValueCategory.Absent => false,
        SourceValueCategory.Object or SourceValueCategory.Function or SourceValueCategory.Symbol => true,
        _ => truthy(value),
    };

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
            System.Numerics.BigInteger number => number.ToString(CultureInfo.InvariantCulture),
            _ => "[object Object]"
        };
    }
}
