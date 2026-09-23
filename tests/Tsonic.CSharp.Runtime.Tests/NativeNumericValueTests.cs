using System;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class NativeNumericValueTests
{
    [Fact]
    public void ClosedIntegersRetainNativeWidthAndPrecision()
    {
        const long exact = 9007199254740993;
        Assert.Equal(exact + 1, Assert.IsType<long>(TsValue.ApplyDynamicBinary(exact, "+", 1L).unwrap()));
        Assert.Equal(exact - 1, Assert.IsType<long>(TsValue.ApplyDynamicBinary(exact, "-", 1L).unwrap()));
        Assert.Equal(ulong.MaxValue, Assert.IsType<ulong>(TsValue.ApplyDynamicBinary(ulong.MaxValue - 1, "+", 1UL).unwrap()));
        Assert.Equal(3L, Assert.IsType<long>(TsValue.ApplyDynamicBinary(7L, "/", 2L).unwrap()));
        Assert.False(TsValue.ApplyDynamicBinaryBoolean(exact, "===", exact - 1));
        Assert.True(TsValue.ApplyDynamicBinaryBoolean(exact, ">", exact - 1));
        Assert.Equal(~exact, Assert.IsType<long>(TsValue.ApplyDynamicUnary(exact, "~").unwrap()));
        Assert.Equal(unchecked(int.MaxValue + 1), Assert.IsType<int>(TsValue.ApplyDynamicBinary(int.MaxValue, "+", 1).unwrap()));
        Assert.Throws<DivideByZeroException>(() => TsValue.ApplyDynamicBinary(1L, "/", 0L));
    }

    [Fact]
    public void ClosedNumericOperationsUseNativePromotions()
    {
        Assert.Equal(3, Assert.IsType<int>(TsValue.ApplyDynamicBinary((byte)1, "+", (short)2).unwrap()));
        Assert.Equal(3L, Assert.IsType<long>(TsValue.ApplyDynamicBinary(1u, "+", 2).unwrap()));
        Assert.Equal(3u, Assert.IsType<uint>(TsValue.ApplyDynamicBinary(1u, "+", (byte)2).unwrap()));
        Assert.Equal(3UL, Assert.IsType<ulong>(TsValue.ApplyDynamicBinary(1UL, "+", 2u).unwrap()));
        Assert.Equal(3.5f, Assert.IsType<float>(TsValue.ApplyDynamicBinary(1.5f, "+", 2).unwrap()));
        Assert.Equal(3.5d, Assert.IsType<double>(TsValue.ApplyDynamicBinary(1.5d, "+", 2f).unwrap()));
        Assert.Equal(3.5m, Assert.IsType<decimal>(TsValue.ApplyDynamicBinary(1.5m, "+", 2L).unwrap()));
        Assert.Throws<TypeError>(() => TsValue.ApplyDynamicBinary(1m, "+", 2d));
        Assert.Throws<TypeError>(() => TsValue.ApplyDynamicBinary(1UL, "+", 2L));
        Assert.Throws<TypeError>(() => TsValue.ApplyDynamicBinary("3", "*", 2));
        Assert.Throws<TypeError>(() => TsValue.ApplyDynamicBinary(true, "*", 2));
        Assert.Throws<TypeError>(() => TsValue.ApplyDynamicUnary(1.0, "~"));
        Assert.False(TsValue.ApplyDynamicBinaryBoolean("2", "==", 2));
        object nan = double.NaN;
        Assert.False(TsValue.ApplyDynamicBinaryBoolean(nan, "===", nan));
        Assert.False(TsValue.ApplyDynamicBinaryBoolean(double.NaN, "<", 1.0));
    }

    [Fact]
    public void ExtendedNativeCarriersRoundTripAndComputeWithoutFloatIntermediates()
    {
        foreach (var value in new object[] { (Half)2, (nint)2, (nuint)2, Int128.MinValue, UInt128.MaxValue })
        {
            Assert.Equal(value, TsValue.from(value).unwrap());
            Assert.True(TsValue.ApplyDynamicBinaryBoolean(value, "===", value));
            Assert.Equal(value, TsValue.ApplyDynamicUnary(value, "+").unwrap());
        }
        Assert.Equal(Int128.MinValue + 1, Assert.IsType<Int128>(TsValue.ApplyDynamicBinary(Int128.MinValue, "+", (Int128)1).unwrap()));
        Assert.Equal(UInt128.MaxValue - 1, Assert.IsType<UInt128>(TsValue.ApplyDynamicBinary(UInt128.MaxValue, "-", (UInt128)1).unwrap()));
        Assert.Equal((nint)3, Assert.IsType<nint>(TsValue.ApplyDynamicBinary((nint)1, "+", (nint)2).unwrap()));
        Assert.Equal((nuint)3, Assert.IsType<nuint>(TsValue.ApplyDynamicBinary((nuint)1, "+", (nuint)2).unwrap()));
        Assert.Equal((Half)3, Assert.IsType<Half>(TsValue.ApplyDynamicBinary((Half)1, "+", (Half)2).unwrap()));
        Assert.False(TsValue.ToDynamicBoolean((UInt128)0));
        Assert.True(TsValue.ToDynamicBoolean(UInt128.MaxValue));
    }
}
