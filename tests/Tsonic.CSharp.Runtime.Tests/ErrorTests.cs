using Xunit;
using System.Runtime.CompilerServices;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class ErrorTests
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Error CreateErrorAtOrigin()
    {
        var error = new TypeError("invalid 😀 value");
        Error.captureStackTrace(error);
        return error;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string? ReadStackElsewhere(Error error) => error.stack;

    [Fact]
    public void ExplicitErrorStackRetainsCaptureFramesAcrossLaterReads()
    {
        var error = CreateErrorAtOrigin();
        var alias = error;
        var stack = ReadStackElsewhere(alias);

        Assert.NotNull(stack);
        Assert.StartsWith("TypeError: invalid 😀 value\n", stack);
        Assert.Contains(nameof(CreateErrorAtOrigin), stack);
        Assert.DoesNotContain(nameof(ReadStackElsewhere), stack);
        Assert.Same(stack, error.stack);
        Assert.Same(error, alias);
    }

    [Fact]
    public void EmptyErrorStackHasNoInventedColonAndExplicitClearingPersists()
    {
        var error = new Error();
        Assert.Null(error.stack);
        Error.captureStackTrace(error);
        Assert.StartsWith("Error\n", error.stack);
        error.stack = null;
        try
        {
            throw error;
        }
        catch (Error caught)
        {
            Assert.Same(error, caught);
            Assert.Null(caught.stack);
            Assert.NotNull(caught.StackTrace);
        }
    }

    [Fact]
    public void ErrorStackHeaderUsesSourcePropertiesAtExplicitCapture()
    {
        var error = new Error("original");
        error.name = "CustomError";
        error.message = "updated";
        Error.captureStackTrace(error);
        var stack = error.stack;

        Assert.StartsWith("CustomError: updated\n", stack);
        error.message = "changed after observation";
        Assert.Same(stack, error.stack);
    }

    [Fact]
    public void OrdinaryErrorsNeverCaptureOnConstructionReadOrThrow()
    {
        foreach (var error in new Error[] { new Error("failure"), new TypeError("type"), new RangeError("range"), new URIError("uri") })
        {
            Assert.Null(error.stack);
            Assert.Null(error.stack);
            try { throw error; }
            catch (Error caught)
            {
                Assert.Same(error, caught);
                Assert.Null(caught.stack);
                Assert.NotNull(caught.StackTrace);
            }
        }
    }

    [Fact]
    public void ExplicitRecaptureReplacesPreviousStackAcrossAliases()
    {
        var error = CreateErrorAtOrigin();
        var previous = error.stack;
        var alias = error;
        error.message = "changed";
        Error.captureStackTrace(alias);
        Assert.NotEqual(previous, error.stack);
        Assert.StartsWith("TypeError: changed\n", error.stack);
        Assert.Contains(nameof(ExplicitRecaptureReplacesPreviousStackAcrossAliases), error.stack);
        Assert.DoesNotContain(nameof(CreateErrorAtOrigin), error.stack);
    }

    [Fact]
    public void DefaultErrorAllocationIsCloseToNativeExceptionAllocation()
    {
        for (var index = 0; index < 100; index++)
        {
            System.GC.KeepAlive(new System.Exception("failure"));
            System.GC.KeepAlive(new Error("failure"));
        }
        var beforeNative = System.GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 1000; index++) System.GC.KeepAlive(new System.Exception("failure"));
        var nativeBytes = System.GC.GetAllocatedBytesForCurrentThread() - beforeNative;
        var beforeSource = System.GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 1000; index++) System.GC.KeepAlive(new Error("failure"));
        var sourceBytes = System.GC.GetAllocatedBytesForCurrentThread() - beforeSource;
        Assert.InRange(sourceBytes, nativeBytes, nativeBytes + 64_000);
    }

    [Fact]
    public void ErrorSourcePropertiesRemainExactAndWritable()
    {
        var error = new Error("original");

        error.name = "NamedError";
        error.message = "updated";
        error.stack = "authored stack";

        Assert.Equal("NamedError", error.name);
        Assert.Equal("updated", error.message);
        Assert.Equal("updated", error.Message);
        Assert.Equal("authored stack", error.stack);
    }

    [Fact]
    public void SpecializedErrorsKeepWritableSourceNames()
    {
        var range = new RangeError("range");
        var type = new TypeError("type");
        var uri = new URIError("uri");

        Assert.Equal("RangeError", range.name);
        Assert.Equal("TypeError", type.name);
        Assert.Equal("URIError", uri.name);

        range.name = "CustomRange";
        type.name = "CustomType";
        uri.name = "CustomUri";

        Assert.Equal("CustomRange", range.name);
        Assert.Equal("CustomType", type.name);
        Assert.Equal("CustomUri", uri.name);
    }
}
