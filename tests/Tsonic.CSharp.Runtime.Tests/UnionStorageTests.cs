using System;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class UnionStorageTests
{
    [Fact]
    public void NumericUnionConstructionAndReadsDoNotAllocate()
    {
        var warm = Union<int, double>.From1(4);
        Assert.Equal(4, warm.As1());
        var before = GC.GetAllocatedBytesForCurrentThread();
        var sum = 0;
        for (var index = 0; index < 10000; index++) sum += Union<int, double>.From1(index).As1();
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(49995000, sum);
        Assert.Equal(0, allocated);
    }

    [Fact]
    public void CopiesNullableValuesAndRepeatedArmTypesRemainExact()
    {
        var original = Union<string?, string?>.From2(null);
        var copy = original;
        Assert.False(copy.Is1());
        Assert.True(copy.Is2());
        Assert.Null(copy.As2());
        Assert.Throws<InvalidOperationException>(() => copy.As1());
        Assert.Throws<InvalidOperationException>(() => default(Union<int, string>).As1());
        Union<int, string>? absent = null;
        Assert.False(absent == 1);
        var wide = Union<int, long, string, bool, double, byte, char, object>.From8(original);
        Assert.True(wide.Is8());
        Assert.Equal(original, Assert.IsType<Union<string?, string?>>(wide.As8()));
    }
}
