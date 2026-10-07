using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class EmptyObjectTests
{
    [Fact]
    public void FrozenStateSurvivesNativeObjectCarrierAliases()
    {
        var first = new EmptyObject();
        object alias = first;
        object second = EmptyObject.Freeze(new EmptyObject());
        Assert.False(EmptyObject.IsFrozen(alias));
        Assert.Same(first, EmptyObject.Freeze(alias));
        Assert.True(EmptyObject.IsFrozen(first));
        Assert.True(EmptyObject.IsFrozen(alias));
        Assert.True(EmptyObject.IsFrozen(second));
        Assert.NotSame(alias, second);
    }

    [Fact]
    public void EmptyObjectOperationsDoNotGuessAnUnknownNativeCarrierOrInventAbsence()
    {
        object value = new object();
        Assert.Throws<System.InvalidCastException>(() => EmptyObject.Freeze(value));
        Assert.Throws<System.InvalidCastException>(() => EmptyObject.IsFrozen(value));
        Assert.Throws<System.NullReferenceException>(() => EmptyObject.Freeze<object>(null!));
        Assert.Throws<System.NullReferenceException>(() => EmptyObject.IsFrozen<object>(null!));
    }

    [Fact]
    public void BroadNativeFreezeOperationsAllocateNoAdditionalState()
    {
        object value = new EmptyObject();
        var observed = false;
        for (var iteration = 0; iteration < 20000; iteration += 1)
        {
            EmptyObject.Freeze(value);
            observed = EmptyObject.IsFrozen(value);
        }
        var before = System.GC.GetAllocatedBytesForCurrentThread();
        for (var iteration = 0; iteration < 20000; iteration += 1)
        {
            EmptyObject.Freeze(value);
            observed = EmptyObject.IsFrozen(value);
        }
        var allocated = System.GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.True(observed);
        Assert.Equal(0, allocated);
    }

    [Fact]
    public void BroadValuesPreserveEmptyIdentityWithoutExposingProperties()
    {
        var first = new EmptyObject();
        var second = new EmptyObject();
        var wrapped = TsValue.from(first);
        Assert.Same(first, wrapped.unwrap());
        Assert.True(TsValue.ApplyDynamicBinaryBoolean(wrapped, "===", TsValue.from(first)));
        Assert.False(TsValue.ApplyDynamicBinaryBoolean(wrapped, "===", TsValue.from(second)));
        Assert.Throws<System.NotSupportedException>(() => wrapped.ReadDynamicSlot("field"));
        Assert.Throws<System.NotSupportedException>(() => TsValue.from(new object()));
    }

    [Fact]
    public void EmptyObjectIdentityAndFrozenStateBelongToTheRetainedObject()
    {
        var first = new EmptyObject();
        var alias = first;
        var second = new EmptyObject();
        Assert.NotSame(first, second);
        Assert.False(EmptyObject.IsFrozen(alias));
        Assert.Same(first, EmptyObject.Freeze(alias));
        Assert.True(EmptyObject.IsFrozen(first));
        Assert.False(EmptyObject.IsFrozen(second));
    }
}
