using System;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class UnionReferenceTests
{
    private class Base { public int Value { get; set; } }
    private sealed class Box : Base { }

    [Fact]
    public void ReferenceProjectionPreservesTheActiveAliasAtEveryArity()
    {
        var value = new Box { Value = 7 };
        Base[] results = [
            Union<Box, Box>.From1(value).AsReference<Base>(),
            Union<Box, Box>.From2(value).AsReference<Base>(),
            Union<Box, Box, Box>.From3(value).AsReference<Base>(),
            Union<Box, Box, Box, Box>.From4(value).AsReference<Base>(),
            Union<Box, Box, Box, Box, Box>.From5(value).AsReference<Base>(),
            Union<Box, Box, Box, Box, Box, Box>.From6(value).AsReference<Base>(),
            Union<Box, Box, Box, Box, Box, Box, Box>.From7(value).AsReference<Base>(),
            Union<Box, Box, Box, Box, Box, Box, Box, Box>.From8(value).AsReference<Base>(),
        ];
        foreach (var result in results) Assert.Same(value, result);
        results[0].Value = 11;
        foreach (var result in results) Assert.Equal(11, result.Value);
    }

    [Fact]
    public void IncompatibleReferenceProjectionFailsAtEveryArity()
    {
        var value = new Box();
        Assert.Throws<InvalidCastException>(() => Union<Box, Box>.From2(value).AsReference<string>());
        Assert.Throws<InvalidCastException>(() => Union<Box, Box, Box>.From3(value).AsReference<string>());
        Assert.Throws<InvalidCastException>(() => Union<Box, Box, Box, Box>.From4(value).AsReference<string>());
        Assert.Throws<InvalidCastException>(() => Union<Box, Box, Box, Box, Box>.From5(value).AsReference<string>());
        Assert.Throws<InvalidCastException>(() => Union<Box, Box, Box, Box, Box, Box>.From6(value).AsReference<string>());
        Assert.Throws<InvalidCastException>(() => Union<Box, Box, Box, Box, Box, Box, Box>.From7(value).AsReference<string>());
        Assert.Throws<InvalidCastException>(() => Union<Box, Box, Box, Box, Box, Box, Box, Box>.From8(value).AsReference<string>());
    }

    [Fact]
    public void ReferenceProjectionDoesNotAllocatePerCall()
    {
        var value = new Box { Value = 7 };
        var union = Union<Box, Box, Box>.From2(value);
        Assert.Same(value, union.AsReference<Base>());
        var before = GC.GetAllocatedBytesForCurrentThread();
        var total = 0;
        for (var index = 0; index < 10_000; index++) total += union.AsReference<Base>().Value;
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(70_000, total);
        Assert.Equal(0, allocated);
    }
}
