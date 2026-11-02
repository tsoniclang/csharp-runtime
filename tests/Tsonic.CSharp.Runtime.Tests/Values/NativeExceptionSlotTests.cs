using System;
using System.Runtime.ExceptionServices;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class NativeExceptionSlotTests
{
    [Fact]
    public void NativeExceptionSlotsKeepRuntimeTypeCauseIdentityAndNativeStackLifecycle()
    {
        var cause = new Exception("cause");
        var original = new InvalidOperationException("original", cause);
        var value = TsValue.from(original);
        Assert.Same(original, value.unwrap());
        Assert.Equal(nameof(InvalidOperationException), value.ReadDynamicSlot("name").unwrap());
        Assert.Equal(original.Message, value.ReadDynamicSlot("message").unwrap());
        Assert.Null(value.ReadDynamicSlot("stack").unwrap());
        Assert.Same(cause, value.ReadDynamicSlot("cause").unwrap());
        ExceptionDispatchInfo.SetCurrentStackTrace(original);
        Assert.Equal(original.StackTrace, value.ReadDynamicSlot("stack").unwrap());
        Assert.Same(cause, value.ReadDynamicSlot("cause").unwrap());
        Assert.Null(TsValue.from(new Exception("absent")).ReadDynamicSlot("cause").unwrap());
    }

    [Fact]
    public void ExplicitRuntimeErrorKeepsItsOwnMutableSlotContract()
    {
        var original = new Error("original") { name = "Explicit", stack = "authored" };
        var value = TsValue.from(original);
        Assert.Same(original, value.unwrap());
        Assert.Equal("Explicit", value.ReadDynamicSlot("name").unwrap());
        Assert.Equal("original", value.ReadDynamicSlot("message").unwrap());
        Assert.Equal("authored", value.ReadDynamicSlot("stack").unwrap());
        original.name = "Changed";
        original.message = "updated";
        Assert.Equal("Changed", value.ReadDynamicSlot("name").unwrap());
        Assert.Equal("updated", value.ReadDynamicSlot("message").unwrap());
    }

    [Fact]
    public void NativeUntracedExceptionNameAndCauseReadsHaveNoWrapperOrDiagnosticAllocation()
    {
        var cause = new Exception("cause");
        var original = new InvalidOperationException("original", cause);
        var value = TsValue.from(original);
        for (var index = 0; index < 1024; index++)
        {
            GC.KeepAlive(value.ReadDynamicSlot("name").unwrap());
            GC.KeepAlive(value.ReadDynamicSlot("cause").unwrap());
        }
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 4096; index++)
        {
            GC.KeepAlive(value.ReadDynamicSlot("name").unwrap());
            GC.KeepAlive(value.ReadDynamicSlot("cause").unwrap());
        }
        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
        Assert.Null(original.StackTrace);
        Assert.Same(original, value.unwrap());
        Assert.Same(cause, original.InnerException);
    }
}
