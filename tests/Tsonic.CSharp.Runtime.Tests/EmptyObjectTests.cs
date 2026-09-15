using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class EmptyObjectTests
{
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
