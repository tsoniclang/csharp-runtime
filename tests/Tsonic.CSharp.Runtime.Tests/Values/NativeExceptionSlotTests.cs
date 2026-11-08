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
        var value = TsValue.fromnative(original);
        Assert.Same(original, value.unwrap());
        Assert.Equal(nameof(InvalidOperationException), value.ReadDynamicSlot("name").unwrap());
        Assert.Equal(original.Message, value.ReadDynamicSlot("message").unwrap());
        Assert.Null(value.ReadDynamicSlot("stack").unwrap());
        Assert.Same(cause, value.ReadDynamicSlot("cause").unwrap());
        ExceptionDispatchInfo.SetCurrentStackTrace(original);
        Assert.Equal(original.StackTrace, value.ReadDynamicSlot("stack").unwrap());
        Assert.Same(cause, value.ReadDynamicSlot("cause").unwrap());
        Assert.Null(TsValue.fromnative(new Exception("absent")).ReadDynamicSlot("cause").unwrap());
    }

    [Fact]
    public void BoxedRuntimeErrorUsesTheNativeExceptionSurface()
    {
        var cause = new Exception("cause");
        var original = new Error("original", cause) { name = "Explicit", stack = "authored" };
        var value = TsValue.fromnative(original);
        Assert.Same(original, value.unwrap());
        Assert.Equal(original.GetType().Name, value.ReadDynamicSlot("name").unwrap());
        Assert.Equal(original.Message, value.ReadDynamicSlot("message").unwrap());
        Assert.Null(value.ReadDynamicSlot("stack").unwrap());
        Assert.Same(cause, value.ReadDynamicSlot("cause").unwrap());
        original.name = "Changed";
        original.message = "updated";
        Assert.Equal(original.GetType().Name, value.ReadDynamicSlot("name").unwrap());
        Assert.Equal(original.Message, value.ReadDynamicSlot("message").unwrap());
        ExceptionDispatchInfo.SetCurrentStackTrace(original);
        Assert.Equal(original.StackTrace, value.ReadDynamicSlot("stack").unwrap());
        Assert.Same(cause, value.ReadDynamicSlot("cause").unwrap());
    }

    [Fact]
    public void RuntimeErrorTypedReferenceKeepsItsOwnMutableMembers()
    {
        var original = new Error("original") { name = "Explicit", stack = "authored" };
        Error alias = original;
        var boxed = TsValue.fromnative(original);
        Assert.Equal("Explicit", alias.name);
        Assert.Equal("original", alias.message);
        Assert.Equal("authored", alias.stack);
        alias.name = "Changed";
        alias.message = "updated";
        alias.stack = "updated stack";
        Assert.Same(original, alias);
        Assert.Same(original, boxed.unwrap());
        Assert.Equal("Changed", original.name);
        Assert.Equal("updated", original.message);
        Assert.Equal("updated stack", original.stack);
        Assert.Equal(original.GetType().Name, boxed.ReadDynamicSlot("name").unwrap());
        Assert.Equal(original.Message, boxed.ReadDynamicSlot("message").unwrap());
        Assert.Null(boxed.ReadDynamicSlot("stack").unwrap());
    }

    [Fact]
    public void NativeExceptionStackObservationEvaluatesItsVirtualGetterOnce()
    {
        var original = new ObservedException();
        var value = TsValue.fromnative(original);
        Assert.Equal("native stack", value.ReadDynamicSlot("stack").unwrap());
        Assert.Equal(1, original.Reads);
        Assert.Same(original, value.unwrap());
    }

    [Fact]
    public void NativeUntracedExceptionNameAndCauseReadsHaveNoWrapperOrDiagnosticAllocation()
    {
        var cause = new Exception("cause");
        var original = new InvalidOperationException("original", cause);
        var value = TsValue.fromnative(original);
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

    private sealed class ObservedException : Exception
    {
        public int Reads { get; private set; }
        public override string? StackTrace
        {
            get
            {
                Reads++;
                return "native stack";
            }
        }
    }
}
