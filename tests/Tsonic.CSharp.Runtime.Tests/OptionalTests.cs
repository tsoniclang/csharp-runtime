using System;
using System.Runtime.CompilerServices;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class OptionalTests
{
    [Fact]
    public void AbsenceIsDistinctFromTheDefaultNativeValue()
    {
        Optional<int, Undefined> absent = default;
        Optional<int, Undefined> zero = 0;
        Assert.True(absent.Is1());
        Assert.False(absent.Is2());
        Assert.Same(Undefined.value, absent.As1());
        Assert.True(zero.Is2());
        Assert.False(zero.Is1());
        Assert.Equal(0, zero.As2());
        Assert.NotEqual(absent, zero);
        Assert.Throws<InvalidOperationException>(() => absent.As2());
        Assert.Throws<InvalidOperationException>(() => zero.As1());
    }

    [Fact]
    public void ClosedAbsenceTagsRetainTheirExactIdentity()
    {
        var missing = Optional<long, Undefined>.From1(Undefined.value);
        var empty = Optional<long, Null>.From1(Null.value);
        Assert.Same(Undefined.value, missing.As1());
        Assert.Same(Null.value, empty.As1());
        Assert.Equal("undefined", missing.ToString());
        Assert.Equal("null", empty.ToString());
        Assert.True(missing == Undefined.value);
        Assert.True(Undefined.value == missing);
        Assert.True(empty == Null.value);
        Assert.True(Null.value == empty);
    }

    [Fact]
    public void NativeIntegerValuesRemainExact()
    {
        foreach (var native in new[] { long.MinValue, -9007199254740993L, 0, 9007199254740993L, long.MaxValue })
        {
            var optional = Optional<long, Undefined>.From2(native);
            Assert.Equal(native, optional.As2());
            Assert.True(optional == native);
            Assert.True(native == optional);
            Assert.False(optional == Undefined.value);
            Assert.False(Undefined.value == optional);
            Assert.True(optional != Undefined.value);
            Assert.Equal(optional, Optional<long, Undefined>.From2(native));
            Assert.Equal(optional.GetHashCode(), Optional<long, Undefined>.From2(native).GetHashCode());
        }
    }

    [Fact]
    public void ReferencesRetainAliasingAndPresentNullIsNotAbsence()
    {
        var reference = new MutableValue();
        var optional = Optional<MutableValue, Undefined>.From2(reference);
        reference.Count = 7;
        Assert.Same(reference, optional.As2());
        Assert.Equal(7, optional.As2().Count);
        var presentNull = Optional<string?, Undefined>.From2(null);
        Assert.True(presentNull.Is2());
        Assert.Null(presentNull.As2());
        Assert.NotEqual(default(Optional<string?, Undefined>), presentNull);
    }

    [Fact]
    public void ClosedNullableBoundariesPreserveValuesAndAbsence()
    {
        Assert.Null(Optional.ToNullable(Optional.FromNullable<long, Undefined>(null)));
        Assert.Equal(9007199254740993L, Optional.ToNullable(Optional.FromNullable<long, Undefined>(9007199254740993L)));
        var reference = new MutableValue();
        Assert.Same(reference, Optional.ToReference(Optional.FromReference<MutableValue, Undefined>(reference)));
        Assert.Null(Optional.ToReference(Optional.FromReference<MutableValue, Undefined>(null)));
    }

    [Fact]
    public void ValueStorageContainsNoManagedReferenceForTheAbsentTag()
    {
        Assert.False(RuntimeHelpers.IsReferenceOrContainsReferences<Optional<long, Undefined>>());
        Assert.False(RuntimeHelpers.IsReferenceOrContainsReferences<Optional<long, Null>>());
        Assert.Equal(Unsafe.SizeOf<long?>(), Unsafe.SizeOf<Optional<long, Undefined>>());
    }

    [Fact]
    public void ConstructionAndProjectionAllocateNothing()
    {
        var warm = SumValues(1000);
        var before = GC.GetAllocatedBytesForCurrentThread();
        var result = SumValues(10000);
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(499500, warm);
        Assert.Equal(49995000, result);
        Assert.Equal(0, allocated);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static long SumValues(int count)
    {
        long sum = 0;
        for (var index = 0; index < count; index++)
        {
            var value = Optional<long, Undefined>.From2(index);
            if (value.Is2()) sum += value.As2();
        }
        return sum;
    }

    private sealed class MutableValue
    {
        public int Count;
    }
}
