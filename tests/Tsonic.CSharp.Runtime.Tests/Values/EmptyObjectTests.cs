using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class EmptyObjectTests
{
    [Fact]
    public void NativeObjectCarrierAliasesRetainEmptyIdentity()
    {
        var first = new EmptyObject();
        object alias = first;
        object second = new EmptyObject();
        Assert.Same(first, alias);
        Assert.NotSame(alias, second);
    }

    [Fact]
    public void BroadNativeIdentityOperationsAllocateNoAdditionalState()
    {
        object value = new EmptyObject();
        object observed = value;
        for (var iteration = 0; iteration < 20000; iteration += 1)
        {
            observed = value;
        }
        var before = System.GC.GetAllocatedBytesForCurrentThread();
        for (var iteration = 0; iteration < 20000; iteration += 1)
        {
            observed = value;
        }
        var allocated = System.GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Same(value, observed);
        Assert.Equal(0, allocated);
    }

    [Fact]
    public void BroadValuesPreserveEmptyIdentityWithoutExposingProperties()
    {
        var first = new EmptyObject();
        var second = new EmptyObject();
        var wrapped = TsValue.from(first);
        Assert.Same(first, wrapped.unwrap());
        Assert.True(TsValue.StrictEqual(wrapped, TsValue.from(first)));
        Assert.False(TsValue.StrictEqual(wrapped, TsValue.from(second)));
        Assert.Throws<System.NotSupportedException>(() => wrapped.ReadDynamicSlot("field"));
        Assert.Throws<System.NotSupportedException>(() => TsValue.from(new object()));
    }

    [Fact]
    public void EmptyObjectIdentityBelongsToTheRetainedObject()
    {
        var first = new EmptyObject();
        var alias = first;
        var second = new EmptyObject();
        Assert.NotSame(first, second);
        Assert.Same(first, alias);
    }
}
