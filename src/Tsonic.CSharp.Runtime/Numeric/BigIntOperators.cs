using System;
using System.Numerics;

namespace Tsonic.CSharp.Runtime;

public static class BigIntOperators
{
    public static BigInteger LeftShift(BigInteger value, BigInteger count) => Shift(value, count, true);

    public static BigInteger RightShift(BigInteger value, BigInteger count) => Shift(value, count, false);

    public static BigInteger Divide(BigInteger value, BigInteger divisor) =>
        divisor.IsZero ? throw new RangeError("Division by zero") : value / divisor;

    public static BigInteger Remainder(BigInteger value, BigInteger divisor) =>
        divisor.IsZero ? throw new RangeError("Division by zero") : value % divisor;

    private static BigInteger Shift(BigInteger value, BigInteger count, bool left)
    {
        if (value.IsZero || count.IsZero) return value;
        if (count.Sign < 0)
        {
            left = !left;
            count = BigInteger.Negate(count);
        }
        var bits = BigInteger.Abs(value).GetBitLength();
        if (!left && count >= bits) return value.Sign < 0 ? BigInteger.MinusOne : BigInteger.Zero;
        if (left && count > int.MaxValue - bits)
            throw new RangeError("BigInt shift exceeds native storage");
        try
        {
            return left ? value << (int)count : value >> (int)count;
        }
        catch (OverflowException)
        {
            throw new RangeError("BigInt shift exceeds native storage");
        }
    }
}
