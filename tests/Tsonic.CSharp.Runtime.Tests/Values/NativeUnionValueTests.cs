using System;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class NativeUnionValueTests
{
    [Fact]
    public void NativeUnionAdmissionRejectsOpenActivePayloadsAtEveryArity()
    {
        var open = new OpenPayload();
        object[] invalid = {
            Union<int, OpenPayload>.From2(open),
            Union<int, string, OpenPayload>.From3(open),
            Union<int, string, bool, OpenPayload>.From4(open),
            Union<int, string, bool, byte, OpenPayload>.From5(open),
            Union<int, string, bool, byte, long, OpenPayload>.From6(open),
            Union<int, string, bool, byte, long, short, OpenPayload>.From7(open),
            Union<int, string, bool, byte, long, short, double, OpenPayload>.From8(open),
            Union<int, object>.From2(Union<string, OpenPayload>.From2(open)),
        };
        foreach (var value in invalid) Assert.Throws<NotSupportedException>(() => TsValue.fromnative(value));
        Assert.Equal(7, TsValue.CastDynamic<int>(TsValue.fromnative(Union<int, OpenPayload>.From1(7))));
        Assert.True(TsValue.fromnative(Union<int, OpenPayload?>.From2(null)).isUndefined());
    }

    [Fact]
    public void NativeCategoryQueryDoesNotReplaceAnExistingSourceCategory()
    {
        Assert.Equal(SourceValueCategory.Number, TsValue.nativecategory(7));
        Assert.Equal(SourceValueCategory.BigInt, TsValue.nativecategory(7L));
        Assert.Equal(SourceValueCategory.BigInt, TsValue.nativecategory(System.Numerics.BigInteger.One << 300));
        Assert.Equal(SourceValueCategory.Number, TsValue.nativecategory((int?)7));
        Assert.Equal(SourceValueCategory.Absent, TsValue.nativecategory((int?)null));
        var source = TsValue.fromsource<long, SourceNumberCategory<long>>(7);
        Assert.Equal(SourceValueCategory.Number, TsValue.nativecategory(source));
        var original = Union<TsValue, long>.From1(source);
        Assert.Equal(SourceValueCategory.Number, TsValue.fromnative(original).Category);
        Assert.Equal(SourceValueCategory.BigInt, TsValue.fromnative(Union<TsValue, long>.From2(7)).Category);
        Assert.Throws<NotSupportedException>(() => TsValue.nativecategory(new OpenPayload()));
        Assert.Throws<InvalidOperationException>(() => TsValue.nativecategory(default(Union<int, string>)));
    }

    [Fact]
    public void DeepPreboxedNativeUnionsRetainTheirPayloadAndRejectOpenAndDefaultLeaves()
    {
        object present = 7;
        object open = new OpenPayload();
        object uninitialized = default(Union<int, string>);
        for (var depth = 0; depth < 128; depth++)
        {
            present = Union<int, object>.From2(present);
            open = Union<int, object>.From2(open);
            uninitialized = Union<int, object>.From2(uninitialized);
        }
        var admitted = TsValue.fromnative(present);
        Assert.Equal(SourceValueCategory.Number, admitted.Category);
        Assert.Same(present, admitted.unwrap());
        Assert.Throws<NotSupportedException>(() => TsValue.fromnative(open));
        Assert.Throws<InvalidOperationException>(() => TsValue.fromnative(uninitialized));
        Assert.Throws<NotSupportedException>(() => TsValue.nativecategory(open));
        Assert.Throws<InvalidOperationException>(() => TsValue.nativecategory(uninitialized));
    }

    [Fact]
    public void TypedPayloadAdmissionAddsNoBoxingBeyondTheOriginalNativeUnion()
    {
        AssertNativeUnionBoxingOnly(42);
        AssertNativeUnionBoxingOnly(ulong.MaxValue);
        AssertNativeUnionBoxingOnly((int?)42);
        AssertNativeUnionBoxingOnly((int?)null);
    }

    private static void AssertNativeUnionBoxingOnly<Payload>(Payload payload)
    {
        var original = Union<Payload, string>.From1(payload);
        for (var index = 0; index < 1000; index++) GC.KeepAlive(TsValue.fromnative(original));
        var outputs = new object[1000];
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = original;
        var nativeBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.fromnative(original).unwrap()!;
        var selectedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.True(nativeBytes > 0);
        Assert.Equal(nativeBytes, selectedBytes);
        GC.KeepAlive(outputs);
    }

    private sealed class OpenPayload
    {
        public string Value { get; } = "not a closed value carrier";
    }

    [Fact]
    public void EveryNativeUnionArityRetainsItsExactActivePayloadAndOriginalCarrier()
    {
        object[] values = {
            Union<int, string>.From2("ready"),
            Union<int, bool, string>.From3("ready"),
            Union<int, bool, byte, string>.From4("ready"),
            Union<int, bool, byte, long, string>.From5("ready"),
            Union<int, bool, byte, long, short, string>.From6("ready"),
            Union<int, bool, byte, long, short, double, string>.From7("ready"),
            Union<int, bool, byte, long, short, double, ulong, string>.From8("ready"),
        };
        foreach (var original in values)
        {
            var value = TsValue.fromnative(original);
            Assert.Same(original, value.unwrap());
            Assert.Equal("ready", TsValue.UnwrapClosedValue(value));
            Assert.Equal("string", TsValue.ApplyDynamicTypeof(value));
            Assert.Equal("ready", TsValue.CastDynamic<string>(value));
        }
        Assert.True(TsValue.CastDynamic<Union<int, string>>(TsValue.fromnative(values[0])).Is2());
        Assert.True(TsValue.CastDynamic<Union<int, bool, byte, long, short, double, ulong, string>>(TsValue.fromnative(values[6])).Is8());
        Assert.Throws<TypeError>(() => TsValue.CastDynamic<Union<string, int>>(TsValue.fromnative(values[0])));
    }

    [Fact]
    public void TypedAndBoxedUnionAdmissionHaveOneNativeRepresentation()
    {
        Assert.IsType<Union<int, string>>(TsValue.fromnative(Union<int, string>.From2("ready")).unwrap());
        Assert.IsType<Union<int, bool, string>>(TsValue.fromnative(Union<int, bool, string>.From3("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, string>>(TsValue.fromnative(Union<int, bool, byte, string>.From4("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, string>>(TsValue.fromnative(Union<int, bool, byte, long, string>.From5("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, short, string>>(TsValue.fromnative(Union<int, bool, byte, long, short, string>.From6("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, short, double, string>>(TsValue.fromnative(Union<int, bool, byte, long, short, double, string>.From7("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, short, double, ulong, string>>(TsValue.fromnative(Union<int, bool, byte, long, short, double, ulong, string>.From8("ready")).unwrap());
        Assert.True(TsValue.fromnative((Union<int, string>?)null).isUndefined());
        Assert.True(TsValue.fromnative((Union<int, bool, string>?)null).isUndefined());
        Assert.True(TsValue.fromnative((Union<int, bool, byte, string>?)null).isUndefined());
        Assert.True(TsValue.fromnative((Union<int, bool, byte, long, string>?)null).isUndefined());
        Assert.True(TsValue.fromnative((Union<int, bool, byte, long, short, string>?)null).isUndefined());
        Assert.True(TsValue.fromnative((Union<int, bool, byte, long, short, double, string>?)null).isUndefined());
        Assert.True(TsValue.fromnative((Union<int, bool, byte, long, short, double, ulong, string>?)null).isUndefined());
    }

    [Fact]
    public void UninitializedNativeUnionsCannotCrossTheClosedValueBoundary()
    {
        object[] values = {
            default(Union<int, string>), default(Union<int, bool, string>),
            default(Union<int, bool, byte, string>), default(Union<int, bool, byte, long, string>),
            default(Union<int, bool, byte, long, short, string>),
            default(Union<int, bool, byte, long, short, double, string>),
            default(Union<int, bool, byte, long, short, double, ulong, string>),
        };
        foreach (var value in values) Assert.Throws<InvalidOperationException>(() => TsValue.fromnative(value));
        Assert.Throws<InvalidOperationException>(() => TsValue.fromnative(default(Union<int, string>)));
    }

    [Fact]
    public void NestedRepeatedAbsentAndExactIntegerPayloadsPreserveNativeMeaning()
    {
        var original = Union<string?, string?>.From2(null);
        var native = Union<int, object>.From2(original);
        var nested = TsValue.fromnative(TsUnion.From(2, 2, native));
        Assert.Null(TsValue.UnwrapClosedValue(nested));
        Assert.True(nested.isUndefined());
        Assert.Equal(original, TsValue.CastDynamic<Union<string?, string?>>(TsValue.fromnative(original)));
        var exact = TsValue.fromnative(Union<double, ulong>.From2(18_446_744_073_709_551_615UL));
        Assert.Equal(ulong.MaxValue, TsValue.CastDynamic<ulong>(exact));
        Assert.Equal("bigint", TsValue.ApplyDynamicTypeof(exact));
        Assert.True(TsValue.StrictEqual(exact, ulong.MaxValue));
    }

    [Fact]
    public void AdmissionHasOnlyNativeValueBoxingAndReferencePayloadQueriesDoNotAllocate()
    {
        var original = Union<int, string>.From2("ready");
        var outputs = new object?[1000];
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.fromnative(original).unwrap();
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = original;
        var nativeBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.fromnative(original).unwrap();
        var generatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.True(nativeBytes > 0);
        Assert.Equal(nativeBytes, generatedBytes);
        var erased = TsValue.fromnative(outputs[0]);
        var matches = 0;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 1000; index++)
        {
            if (ReferenceEquals(TsValue.UnwrapClosedValue(erased), original.As2())) matches++;
            if (TsValue.ApplyDynamicTypeof(erased) == "string") matches++;
        }
        var queryBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(2000, matches);
        Assert.Equal(0, queryBytes);
        GC.KeepAlive(outputs);
    }
}
