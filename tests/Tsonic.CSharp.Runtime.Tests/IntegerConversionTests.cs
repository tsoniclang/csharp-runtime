using System;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class IntegerConversionTests
{
    [Fact]
    public void ExactNativeValuesRetainWidthsAndAbsence()
    {
        const ulong wide = 9007199254740993;
        Assert.Equal(wide, IntegerConversions.Checked<long, ulong>((long)wide));
        Assert.Equal(ulong.MaxValue, IntegerConversions.Checked<UInt128, ulong>(ulong.MaxValue));
        Assert.Equal(long.MinValue, IntegerConversions.Checked<double, long>(long.MinValue));
        Assert.Equal((byte)17, IntegerConversions.Checked<double, byte>(17));
        Assert.Equal((sbyte)17, IntegerConversions.Checked<float, sbyte>(17));
        Assert.Equal((short)17, IntegerConversions.Checked<double, short>(17));
        Assert.Equal((ushort)17, IntegerConversions.Checked<double, ushort>(17));
        Assert.Equal(17, IntegerConversions.Checked<double, int>(17));
        Assert.Equal(17u, IntegerConversions.Checked<double, uint>(17));
        Assert.Equal((nuint)17, IntegerConversions.Checked<double, nuint>(17));
        Assert.Equal((nint)17, IntegerConversions.Checked<double, nint>(17));
        Assert.Equal((Int128)17, IntegerConversions.Checked<decimal, Int128>(17));
        Assert.Equal((UInt128)17, IntegerConversions.Checked<double, UInt128>(17));
        Assert.Null(IntegerConversions.CheckedNullable<double, int>(null));
        Assert.Equal(17, IntegerConversions.CheckedNullable<double, int>(17));
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(18446744073709551616d)]
    [InlineData(-1d)]
    public void InvalidValuesFailBeforeNativeIntegerStorage(double value)
    {
        Assert.Throws<OverflowException>(() => IntegerConversions.Checked<double, ulong>(value));
        Assert.Throws<OverflowException>(() => IntegerConversions.CheckedNullable<double, ulong>(value));
    }

    [Fact]
    public void NarrowingChecksNativeBoundsWithoutRounding()
    {
        Assert.Throws<OverflowException>(() => IntegerConversions.Checked<int, byte>(256));
        Assert.Throws<OverflowException>(() => IntegerConversions.Checked<int, sbyte>(128));
        Assert.Throws<OverflowException>(() => IntegerConversions.Checked<uint, int>(uint.MaxValue));
        Assert.Throws<OverflowException>(() => IntegerConversions.Checked<double, long>(9223372036854775808d));
        Assert.Throws<OverflowException>(() => IntegerConversions.Checked<UInt128, ulong>((UInt128)ulong.MaxValue + 1));
    }

    [Fact]
    public void SuccessfulConversionsAllocateNoManagedStorage()
    {
        var expected = IntegerConversions.Checked<ulong, ulong>(9007199254740993);
        var before = GC.GetAllocatedBytesForCurrentThread();
        ulong actual = 0;
        for (var index = 0; index < 10000; index++) actual = IntegerConversions.Checked<ulong, ulong>(expected);
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(expected, actual);
        Assert.Equal(0, allocated);
    }
}
