using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class EmptyObjectTests
{
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
