using System.Numerics;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class BigIntOperatorsTests
{
    [Fact]
    public void ShiftsRetainSignPrecisionAndReversedCounts()
    {
        var wide = BigInteger.Parse("9007199254740993");
        Assert.Equal(wide * 4, BigIntOperators.LeftShift(wide, 2));
        Assert.Equal(wide, BigIntOperators.RightShift(wide * 4, 2));
        Assert.Equal(new BigInteger(-3), BigIntOperators.RightShift(-9, 2));
        Assert.Equal(new BigInteger(-3), BigIntOperators.LeftShift(-9, -2));
        Assert.Equal(new BigInteger(12), BigIntOperators.RightShift(3, -2));
    }

    [Fact]
    public void HugeCountsAreResolvedWithoutNarrowingOrAllocatingAnImpossibleValue()
    {
        var huge = BigInteger.One << 100;
        Assert.Equal(BigInteger.Zero, BigIntOperators.RightShift(7, huge));
        Assert.Equal(BigInteger.MinusOne, BigIntOperators.RightShift(-7, huge));
        Assert.Equal(BigInteger.Zero, BigIntOperators.LeftShift(7, -huge));
        Assert.Equal(BigInteger.MinusOne, BigIntOperators.LeftShift(-7, -huge));
        Assert.Equal(BigInteger.Zero, BigIntOperators.LeftShift(0, huge));
        Assert.Equal(BigInteger.Zero, BigIntOperators.RightShift(0, -huge));
        Assert.Throws<RangeError>(() => BigIntOperators.LeftShift(1, huge));
        Assert.Throws<RangeError>(() => BigIntOperators.RightShift(1, -huge));
    }

    [Fact]
    public void DivisionUsesIntegerTruncationAndExplicitZeroErrors()
    {
        Assert.Equal(new BigInteger(-2), BigIntOperators.Divide(-7, 3));
        Assert.Equal(new BigInteger(-1), BigIntOperators.Remainder(-7, 3));
        Assert.Throws<RangeError>(() => BigIntOperators.Divide(1, 0));
        Assert.Throws<RangeError>(() => BigIntOperators.Remainder(1, 0));
    }
}
