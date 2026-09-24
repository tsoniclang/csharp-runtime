using System;
using System.Runtime.CompilerServices;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class OptionalStorageTests
{
    [Fact]
    public void AbsenceIsDistinctFromZeroFalseAndEmptyString()
    {
        int? absent = ValueOptionalStorage<int>.From1(null);
        int? zero = ValueOptionalStorage<int>.From2(0);
        Assert.True(ValueOptionalStorage<int>.Is1(absent));
        Assert.False(ValueOptionalStorage<int>.Is2(absent));
        Assert.Null(ValueOptionalStorage<int>.As1(absent));
        Assert.True(ValueOptionalStorage<int>.Is2(zero));
        Assert.False(ValueOptionalStorage<int>.Is1(zero));
        Assert.Equal(0, ValueOptionalStorage<int>.As2(zero));
        Assert.NotEqual(absent, zero);
        Assert.Throws<InvalidOperationException>(() => ValueOptionalStorage<int>.As2(absent));
        Assert.Throws<InvalidOperationException>(() => ValueOptionalStorage<int>.As1(zero));
        Assert.True(ValueOptionalStorage<bool>.Is2(false));
        Assert.False(ValueOptionalStorage<bool>.As2(false));
        Assert.True(ReferenceOptionalStorage<string>.Is2(""));
        Assert.Equal("", ReferenceOptionalStorage<string>.As2(""));
    }

    [Fact]
    public void SourceAbsenceHasOneNativeValue()
    {
        Assert.Null(AbsentOptionalStorage.From1(null));
        Assert.Null(AbsentOptionalStorage.From2(null));
        Assert.True(AbsentOptionalStorage.Is1(null));
        Assert.False(AbsentOptionalStorage.Is2(null));
        Assert.Null(AbsentOptionalStorage.As1(null));
        Assert.Throws<InvalidOperationException>(() => AbsentOptionalStorage.As2(null));
        var broadNull = TsValue.from(null);
        var broadUndefined = TsValue.undefined();
        Assert.Null(broadNull.unwrap());
        Assert.Null(broadUndefined.unwrap());
        Assert.True(TsValue.ApplyDynamicBinaryBoolean(broadNull, "===", broadUndefined));
        Assert.True(JsValueOptionalStorage.Is1(broadNull));
        Assert.True(JsValueOptionalStorage.Is1(broadUndefined));
        Assert.Null(JsValueOptionalStorage.As1(broadNull));
        Assert.Throws<InvalidOperationException>(() => JsValueOptionalStorage.As2(broadUndefined));
        Assert.True(JsValueOptionalStorage.Is2(TsValue.from(0)));
        Assert.True(JsValueOptionalStorage.Is2(TsValue.from(false)));
        Assert.True(JsValueOptionalStorage.Is2(TsValue.from("")));
    }

    [Fact]
    public void NativeIntegerValuesRemainExact()
    {
        foreach (var native in new[] { long.MinValue, -9007199254740993L, 0, 9007199254740993L, long.MaxValue })
        {
            var optional = ValueOptionalStorage<long>.From2(native);
            Assert.Equal(native, ValueOptionalStorage<long>.As2(optional));
            Assert.Equal(native, optional);
            Assert.False(ValueOptionalStorage<long>.Is1(optional));
            Assert.Equal(optional, ValueOptionalStorage<long>.From2(native));
            Assert.Equal(optional.GetHashCode(), ValueOptionalStorage<long>.From2(native).GetHashCode());
        }
    }

    [Fact]
    public void ReferencesRetainAliasing()
    {
        var reference = new MutableValue();
        var optional = ReferenceOptionalStorage<MutableValue>.From2(reference);
        reference.Count = 7;
        Assert.Same(reference, ReferenceOptionalStorage<MutableValue>.As2(optional));
        Assert.Equal(7, ReferenceOptionalStorage<MutableValue>.As2(optional).Count);
        Assert.Null(ReferenceOptionalStorage<MutableValue>.From1(null));
        Assert.True(ReferenceOptionalStorage<MutableValue>.Is1(null));
        Assert.Throws<InvalidOperationException>(() => ReferenceOptionalStorage<MutableValue>.As2(null));
        Assert.Null(ReferenceOptionalStorage<string?>.From2(null));
        Assert.True(ReferenceOptionalStorage<string?>.Is1(null));
    }

    [Fact]
    public void NullableStorageIsIdempotentAndNativeArraysRetainAliases()
    {
        Assert.Null(NullableValueOptionalStorage<long>.From2(null));
        Assert.Equal(9007199254740993L, NullableValueOptionalStorage<long>.From2(9007199254740993L));
        Assert.Throws<InvalidOperationException>(() => NullableValueOptionalStorage<long>.As2(null));
        long?[] values = [0, null];
        var alias = values;
        Fill<long, long?, ValueOptionalStorage<long>>(values, 9007199254740993L);
        Assert.Same(values, alias);
        Assert.Equal(9007199254740993L, alias[0]);
        Assert.Null(alias[1]);
        var reference = new MutableValue();
        MutableValue?[] references = [null, null];
        Fill<MutableValue, MutableValue?, ReferenceOptionalStorage<MutableValue>>(references, reference);
        Assert.Same(reference, references[0]);
        Assert.Null(references[1]);
    }

    [Fact]
    public void StorageIsNativeAndTheOperationsWitnessHasNoManagedState()
    {
        Assert.False(RuntimeHelpers.IsReferenceOrContainsReferences<long?>());
        Assert.False(RuntimeHelpers.IsReferenceOrContainsReferences<ValueOptionalStorage<long>>());
        Assert.False(RuntimeHelpers.IsReferenceOrContainsReferences<ReferenceOptionalStorage<MutableValue>>());
        Assert.Equal(1, Unsafe.SizeOf<ValueOptionalStorage<long>>());
        Assert.Equal(IntPtr.Size, Unsafe.SizeOf<MutableValue?>());
    }

    [Fact]
    public void GenericConstructionAndProjectionAllocateNothing()
    {
        var warm = SumValues<long?, ValueOptionalStorage<long>>(1000);
        var before = GC.GetAllocatedBytesForCurrentThread();
        var result = SumValues<long?, ValueOptionalStorage<long>>(10000);
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(499500, warm);
        Assert.Equal(49995000, result);
        Assert.Equal(0, allocated);
    }

    private static void Fill<T, TStorage, TOperations>(TStorage[] values, T value)
        where TOperations : struct, IOptionalStorage<T, TStorage>
    {
        values[0] = TOperations.From2(value);
        values[1] = TOperations.From1(null);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static long SumValues<TStorage, TOperations>(int count)
        where TOperations : struct, IOptionalStorage<long, TStorage>
    {
        long sum = 0;
        for (var index = 0; index < count; index++)
        {
            var value = TOperations.From2(index);
            if (TOperations.Is2(value)) sum += TOperations.As2(value);
        }
        return sum;
    }

    private sealed class MutableValue
    {
        public int Count;
    }
}
