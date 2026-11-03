using System;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class NativeDelegateValueTests
{
    [Fact]
    public void NativeDelegatesKeepTheirOriginalReceiverClosedSignatureAndReferenceIdentity()
    {
        var owner = new Receiver();
        Func<int, int> original = new(owner.Read);
        Func<int, int> equivalent = new(owner.Read);
        Assert.True(original.Equals(equivalent));
        Assert.False(ReferenceEquals(original, equivalent));
        var value = TsValue.from(original);
        Assert.Same(original, value.unwrap());
        Assert.Same(original, TsValue.CastDynamic<Func<int, int>>(value));
        Assert.Equal("function", TsValue.ApplyDynamicTypeof(value));
        Assert.True(TsValue.StrictEqual(value, TsValue.from(original)));
        Assert.False(TsValue.StrictEqual(value, TsValue.from(equivalent)));
        Assert.True(TsValue.StrictNotEqual(value, TsValue.from(equivalent)));
        owner.Total = 7;
        Assert.Equal(10, TsValue.CastDynamic<Func<int, int>>(value)(3));
        Assert.Equal(10, owner.Total);
        Assert.Equal(1, owner.Calls);
        Action action = owner.Tick;
        var storedAction = TsValue.from(action);
        Assert.Same(action, storedAction.unwrap());
        TsValue.CastDynamic<Action>(storedAction)();
        Assert.Equal(11, owner.Total);
        Assert.Equal(2, owner.Calls);
        var firstList = original + original;
        var secondList = equivalent + equivalent;
        Assert.True(firstList.Equals(secondList));
        Assert.False(TsValue.StrictEqual(TsValue.from(firstList), TsValue.from(secondList)));
        Assert.True(TsValue.StrictEqual(TsValue.from(firstList), TsValue.from(firstList)));
    }

    [Fact]
    public void NativeDelegateAdmissionDoesNotCreateDynamicInvocationOrEraseContracts()
    {
        Func<long, long> integer = Echo<long>;
        Func<string, string> text = Echo<string>;
        var value = TsValue.from(integer);
        Assert.Same(integer, TsValue.CastDynamic<Func<long, long>>(value));
        Assert.Equal(9007199254740993L, TsValue.CastDynamic<Func<long, long>>(value)(9007199254740993L));
        Assert.Equal("function", TsValue.ApplyDynamicTypeof(TsValue.from(text)));
        Assert.False(TsValue.StrictEqual(value, TsValue.from(text)));
        Assert.Throws<TypeError>(() => TsValue.CastDynamic<Func<int, int>>(value));
        var refusal = Assert.Throws<TypeError>(() => value.InvokeDynamic(7));
        Assert.Equal("Value is not callable.", refusal.Message);
        Assert.Throws<NotSupportedException>(() => TsValue.from(new object()));
        var error = new InvalidOperationException("original native failure");
        Func<int> throwing = () => throw error;
        var observed = Assert.Throws<InvalidOperationException>(() => TsValue.CastDynamic<Func<int>>(TsValue.from(throwing))());
        Assert.Same(error, observed);
        Assert.True(TsValue.StrictEqual(TsValue.undefined(), TsValue.from(null)));
        Assert.Equal("object", TsValue.ApplyDynamicTypeof(TsValue.undefined()));
    }

    [Fact]
    public void NativeDelegateValuesAddNoWrapperCopyOrPerReadAllocation()
    {
        var owner = new Receiver();
        Func<int, int> original = new(owner.Read);
        Func<int, int> equivalent = new(owner.Read);
        for (var index = 0; index < 16384; index++)
        {
            var value = TsValue.from(original);
            _ = TsValue.StrictEqual(value, TsValue.from(original));
            _ = TsValue.StrictEqual(value, TsValue.from(equivalent));
            _ = TsValue.CastDynamic<Func<int, int>>(value);
            _ = TsValue.ApplyDynamicTypeof(value);
        }
        var matches = 0;
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 16384; index++)
        {
            var value = TsValue.from(original);
            if (TsValue.StrictEqual(value, TsValue.from(original)) &&
                !TsValue.StrictEqual(value, TsValue.from(equivalent)) &&
                ReferenceEquals(original, TsValue.CastDynamic<Func<int, int>>(value)) &&
                TsValue.ApplyDynamicTypeof(value) == "function") matches++;
        }
        var selectedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        var nativeMatches = 0;
        before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 16384; index++)
            if (ReferenceEquals(original, original) && !ReferenceEquals(original, equivalent)) nativeMatches++;
        var nativeBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(16384, matches);
        Assert.Equal(nativeMatches, matches);
        Assert.Equal(0, nativeBytes);
        Assert.Equal(nativeBytes, selectedBytes);
        Assert.Equal(0, owner.Calls);
        Assert.Equal(0, owner.Total);
    }

    private static TValue Echo<TValue>(TValue value) => value;

    private sealed class Receiver
    {
        public int Total;
        public int Calls;
        public int Read(int value) { Calls++; Total += value; return Total; }
        public void Tick() { Calls++; Total++; }
    }
}
