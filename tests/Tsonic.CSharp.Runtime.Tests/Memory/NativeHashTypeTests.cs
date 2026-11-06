using System;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class NativeHashTypeTests
{
    [Fact]
    public void PointerHashesHaveNativeUnsignedStorageAndPreserveIdentity()
    {
        var value = 3;
        var location = Location<int>.Bind(new object(), () => value, next => value = next);
        uint hash = Location<int>.Hash(location);
        uint aliasHash = Location<int>.Hash(location);
        uint absentLocation = Location<int>.Hash(null);
        uint absentRaw = RawPointer.Hash(null);
        Assert.Equal(hash, aliasHash);
        Assert.Equal(0U, absentLocation);
        Assert.Equal(0U, absentRaw);
        Assert.Equal(typeof(uint), typeof(Location<int>).GetMethod(nameof(Location<int>.Hash))!.ReturnType);
        Assert.Equal(typeof(uint), typeof(RawPointer).GetMethod(nameof(RawPointer.Hash))!.ReturnType);
    }
}
