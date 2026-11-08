using System;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class SourceEvidenceValueTests
{
    [Fact]
    public void OriginalNumberAndBigIntCategoriesSurviveTheSameNativeLongAndGenericErasure()
    {
        const long original = 9007199254740994;
        var number = Erase<long, SourceNumberCategory<long>>(original);
        var integer = Erase<long, SourceBigIntCategory<long>>(original);
        Assert.Equal(original, TsValue.CastDynamic<long>(number));
        Assert.Equal(original, TsValue.CastDynamic<long>(integer));
        Assert.Equal("number", TsValue.ApplyDynamicTypeof(number));
        Assert.Equal("bigint", TsValue.ApplyDynamicTypeof(integer));
        Assert.False(TsValue.StrictEqual(number, integer));
        Assert.Equal("bigint", TsValue.ApplyDynamicTypeof(TsValue.fromnative(original)));
        Assert.Equal("number", TsValue.ApplyDynamicTypeof(Erase<TsValue, SourceStoredValueCategory>(number)));
        Assert.Throws<ArgumentException>(() => TsValue.fromsource(number, SourceValueCategory.BigInt));
        Assert.Null(typeof(TsValue).GetMethod("from"));
    }

    [Fact]
    public void NativeNullableAndReferenceAbsenceHaveOneStateWithoutLosingPresentFalsyValues()
    {
        var absent = Erase<int?, SourceOptionalValueCategory<int, int?, ValueOptionalStorage<int>, SourceNumberCategory<int>>>(null);
        Assert.True(absent.isUndefined());
        Assert.True(default(TsValue).isUndefined());
        Assert.True(Erase<string?, SourceStringCategory<string?>>(null).isUndefined());
        Assert.Equal(SourceValueCategory.Absent, SourceOptionalValueCategory<int, int?, ValueOptionalStorage<int>, SourceNumberCategory<int>>.Category(null));
        Assert.Equal(SourceValueCategory.Number, SourceOptionalValueCategory<int, int?, ValueOptionalStorage<int>, SourceNumberCategory<int>>.Category(0));
        Assert.Equal(SourceValueCategory.Absent, SourceOptionalValueCategory<string, string?, ReferenceOptionalStorage<string>, SourceStringCategory<string>>.Category(null));
        Assert.Equal(SourceValueCategory.String, SourceOptionalValueCategory<string, string?, ReferenceOptionalStorage<string>, SourceStringCategory<string>>.Category(""));
        Assert.False(Erase<int?, SourceOptionalValueCategory<int, int?, ValueOptionalStorage<int>, SourceNumberCategory<int>>>(0).isUndefined());
        Assert.False(Erase<bool, SourceBooleanCategory<bool>>(false).isUndefined());
        Assert.False(Erase<string, SourceStringCategory<string>>(string.Empty).isUndefined());
        Assert.Throws<ArgumentException>(() => TsValue.fromsource(7, SourceValueCategory.Absent));
    }

    [Fact]
    public void SourceCategorySurvivesDynamicArithmeticAndThrowTransportWithoutNewOperandBoxes()
    {
        var number = Erase<long, SourceNumberCategory<long>>(7);
        var sum = TsValue.Add(number, number);
        Assert.Equal(14L, TsValue.CastDynamic<long>(sum));
        Assert.Equal("number", TsValue.ApplyDynamicTypeof(sum));
        Assert.Equal("number", TsValue.ApplyDynamicTypeof(TsValue.Negate(number)));
        var thrown = TsThrownValueException.from(number);
        var recovered = TsThrownValueException.toValue(thrown);
        Assert.Same(number.unwrap(), recovered.unwrap());
        Assert.Equal(SourceValueCategory.Number, recovered.Category);
        var outputs = new object?[4096];
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.Add(number, number).unwrap();
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = (object)14L;
        var native = GC.GetAllocatedBytesForCurrentThread() - before;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < outputs.Length; index++) outputs[index] = TsValue.Add(number, number).unwrap();
        Assert.Equal(native, GC.GetAllocatedBytesForCurrentThread() - before);
        GC.KeepAlive(outputs);
    }

    [Fact]
    public void ErasedAggregatesKeepTheNativePayloadAndReadItsActualChildCarrier()
    {
        var payload = new System.Collections.Generic.Dictionary<string, object?> { ["value"] = 2L };
        var erased = TsValue.fromsource<System.Collections.Generic.Dictionary<string, object?>,
            SourceObjectCategory<System.Collections.Generic.Dictionary<string, object?>>>(payload);
        Assert.Same(payload, erased.unwrap());
        Assert.Equal(2L, erased.ReadDynamicSlotAs<long>("value"));
        Assert.Equal("bigint", TsValue.ApplyDynamicTypeof(erased.ReadDynamicSlot("value")));
        erased.WriteDynamicSlot("value", TsValue.fromsource<long, SourceNumberCategory<long>>(3));
        Assert.Equal(3L, Assert.IsType<long>(payload["value"]));
        Assert.Equal("bigint", TsValue.ApplyDynamicTypeof(erased.ReadDynamicSlot("value")));
        Assert.Equal(2 * IntPtr.Size, System.Runtime.CompilerServices.Unsafe.SizeOf<TsValue>());
        Assert.Null(typeof(TsValue).Assembly.GetType("Tsonic.CSharp.Runtime.ISourceDynamicView`1"));
        foreach (var method in typeof(TsValue).GetMethods())
            if (method.Name == "fromsource") Assert.InRange(method.GetGenericArguments().Length, 1, 2);
    }

    [Fact]
    public void PartialRolesComposeAndKeepNativeOrderedPayloadsAtomicWithoutAllocating()
    {
        var original = Union<int, int>.From2(19);
        var storage = SourceUnionRole1<Union<int, int>, string>.From(original);
        Assert.True(SourceUnionRole1<Union<int, int>, string>.As(storage).Is2());
        var partial = Union<int, string>.From1(7);
        var complete = PartialRole.From(partial);
        Assert.True(PartialRole.Is(complete));
        Assert.Equal(7, SourceUnionComposedRole<Union<int, string, bool>, Union<int, string>, int, PartialRole, SourceUnionRole1<int, string>>.As(complete));
        Assert.False(PartialRole.Is(Union<int, string, bool>.From3(false)));
        Assert.Throws<InvalidOperationException>(() => PartialRole.As(Union<int, string, bool>.From3(false)));
        for (var index = 0; index < 1000; index++)
        {
            var warm = SourceUnionComposedRole<Union<int, string, bool>, Union<int, string>, int, PartialRole, SourceUnionRole1<int, string>>.From(index);
            _ = SourceUnionComposedRole<Union<int, string, bool>, Union<int, string>, int, PartialRole, SourceUnionRole1<int, string>>.As(warm);
        }
        var before = GC.GetAllocatedBytesForCurrentThread();
        var total = 0;
        for (var index = 0; index < 10000; index++)
        {
            var value = SourceUnionComposedRole<Union<int, string, bool>, Union<int, string>, int, PartialRole, SourceUnionRole1<int, string>>.From(index);
            total += SourceUnionComposedRole<Union<int, string, bool>, Union<int, string>, int, PartialRole, SourceUnionRole1<int, string>>.As(value);
        }
        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
        Assert.Equal(49995000, total);
    }

    private static TsValue Erase<TValue, TCategory>(TValue value) where TCategory : struct, ISourceValueCategory<TValue> =>
        TsValue.fromsource<TValue, TCategory>(value);

    private readonly struct PartialRole : ISourceUnionRole<Union<int, string, bool>, Union<int, string>>
    {
        public static Union<int, string, bool> From(Union<int, string> value) => value.Is1()
            ? Union<int, string, bool>.From1(value.As1()) : Union<int, string, bool>.From2(value.As2());
        public static bool Is(Union<int, string, bool> value) => value.Is1() || value.Is2();
        public static Union<int, string> As(Union<int, string, bool> value) => value.Is1()
            ? Union<int, string>.From1(value.As1()) : Union<int, string>.From2(value.As2());
    }
}
