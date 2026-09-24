using System.Diagnostics;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class NeverTests
{
    [Fact]
    public void InvalidNativeBottomValuesCannotInventResults()
    {
        Assert.Throws<UnreachableException>(() => default(Never).Value<int>());
        Assert.Throws<UnreachableException>(() => default(Never).Value<string>());
    }
}
