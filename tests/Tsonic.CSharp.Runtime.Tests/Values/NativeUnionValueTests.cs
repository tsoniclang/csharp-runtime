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
        foreach (var value in invalid) Assert.Throws<NotSupportedException>(() => TsValue.from(value));
        Assert.Equal(7, TsValue.CastDynamic<int>(TsValue.from(Union<int, OpenPayload>.From1(7))));
        Assert.True(TsValue.from(Union<int, OpenPayload?>.From2(null)).isUndefined());
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
        for (var index = 0; index < 1000; index++) GC.KeepAlive(TsValue.from(original));
        var outputs = new object[1000];
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = original;
        var nativeBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.from(original).unwrap()!;
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
            var value = TsValue.from(original);
            Assert.Same(original, value.unwrap());
            Assert.Equal("ready", TsValue.UnwrapClosedValue(value));
            Assert.Equal("string", TsValue.ApplyDynamicTypeof(value));
            Assert.Equal("ready", TsValue.CastDynamic<string>(value));
        }
        Assert.True(TsValue.CastDynamic<Union<int, string>>(TsValue.from(values[0])).Is2());
        Assert.True(TsValue.CastDynamic<Union<int, bool, byte, long, short, double, ulong, string>>(TsValue.from(values[6])).Is8());
        Assert.Throws<TypeError>(() => TsValue.CastDynamic<Union<string, int>>(TsValue.from(values[0])));
    }

    [Fact]
    public void TypedAndBoxedUnionAdmissionHaveOneNativeRepresentation()
    {
        Assert.IsType<Union<int, string>>(TsValue.from(Union<int, string>.From2("ready")).unwrap());
        Assert.IsType<Union<int, bool, string>>(TsValue.from(Union<int, bool, string>.From3("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, string>>(TsValue.from(Union<int, bool, byte, string>.From4("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, string>>(TsValue.from(Union<int, bool, byte, long, string>.From5("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, short, string>>(TsValue.from(Union<int, bool, byte, long, short, string>.From6("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, short, double, string>>(TsValue.from(Union<int, bool, byte, long, short, double, string>.From7("ready")).unwrap());
        Assert.IsType<Union<int, bool, byte, long, short, double, ulong, string>>(TsValue.from(Union<int, bool, byte, long, short, double, ulong, string>.From8("ready")).unwrap());
        Assert.True(TsValue.from((Union<int, string>?)null).isUndefined());
        Assert.True(TsValue.from((Union<int, bool, string>?)null).isUndefined());
        Assert.True(TsValue.from((Union<int, bool, byte, string>?)null).isUndefined());
        Assert.True(TsValue.from((Union<int, bool, byte, long, string>?)null).isUndefined());
        Assert.True(TsValue.from((Union<int, bool, byte, long, short, string>?)null).isUndefined());
        Assert.True(TsValue.from((Union<int, bool, byte, long, short, double, string>?)null).isUndefined());
        Assert.True(TsValue.from((Union<int, bool, byte, long, short, double, ulong, string>?)null).isUndefined());
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
        foreach (var value in values) Assert.Throws<InvalidOperationException>(() => TsValue.from(value));
        Assert.Throws<InvalidOperationException>(() => TsValue.from(default(Union<int, string>)));
    }

    [Fact]
    public void NestedRepeatedAbsentAndExactIntegerPayloadsPreserveNativeMeaning()
    {
        var original = Union<string?, string?>.From2(null);
        var native = Union<int, object>.From2(original);
        var nested = TsValue.from(TsUnion.From(2, 2, native));
        Assert.Null(TsValue.UnwrapClosedValue(nested));
        Assert.True(nested.isUndefined());
        Assert.Equal(original, TsValue.CastDynamic<Union<string?, string?>>(TsValue.from(original)));
        var exact = TsValue.from(Union<double, ulong>.From2(18_446_744_073_709_551_615UL));
        Assert.Equal(ulong.MaxValue, TsValue.CastDynamic<ulong>(exact));
        Assert.Equal("bigint", TsValue.ApplyDynamicTypeof(exact));
        Assert.True(TsValue.ApplyDynamicBinaryBoolean(exact, "===", ulong.MaxValue));
    }

    [Fact]
    public void AdmissionHasOnlyNativeValueBoxingAndReferencePayloadQueriesDoNotAllocate()
    {
        var original = Union<int, string>.From2("ready");
        var outputs = new object?[1000];
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.from(original).unwrap();
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = original;
        var nativeBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.from(original).unwrap();
        var generatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.True(nativeBytes > 0);
        Assert.Equal(nativeBytes, generatedBytes);
        var erased = TsValue.from(outputs[0]);
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
