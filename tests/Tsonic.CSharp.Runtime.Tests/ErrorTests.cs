using Xunit;
using System.Runtime.CompilerServices;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class ErrorTests
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Error CreateErrorAtOrigin() => new TypeError("invalid 😀 value");

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string? ReadStackElsewhere(Error error) => error.stack;

    [Fact]
    public void UnthrownErrorStackRetainsCreationFramesAcrossLaterReads()
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
    public void ErrorStackHeaderUsesSourcePropertiesOnFirstObservation()
    {
        var error = new Error("original");
        error.name = "CustomError";
        error.message = "updated";
        var stack = error.stack;

        Assert.StartsWith("CustomError: updated\n", stack);
        error.message = "changed after observation";
        Assert.Same(stack, error.stack);
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
