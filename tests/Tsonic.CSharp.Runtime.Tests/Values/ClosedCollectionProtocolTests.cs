using System;
using System.Collections.Generic;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class ClosedCollectionProtocolTests
{
    [Fact]
    public void ClosedObjectReadOnlyProtocolRetainsTheNativeBacking()
    {
        var source = new TsObject();
        IReadOnlyDictionary<string, TsValue> view = source;
        Assert.Same(source, view);
        source.WriteDynamicSlot("count", long.MaxValue);
        Assert.True(view.Count == 1, "The readonly count describes the original native map.");
        Assert.True(view.ContainsKey("count"));
        Assert.True(view.TryGetValue("count", out var value));
        Assert.Equal(long.MaxValue, Assert.IsType<long>(value.unwrap()));
        source.WriteDynamicSlot("count", long.MinValue);
        Assert.Equal(long.MinValue, Assert.IsType<long>(view["count"].unwrap()));
        Assert.False(view.TryGetValue("missing", out _));
        Assert.Throws<KeyNotFoundException>(() => view["missing"]);
        Assert.Equal(new[] { "count" }, view.Keys);
        Assert.Single(view.Values);
        Assert.Single(view);
    }

    [Fact]
    public void ClosedObjectTypedEnumerationDoesNotAllocateOrCopy()
    {
        var source = new TsObject();
        source.WriteDynamicSlot("count", "kept");
        for (var iteration = 0; iteration < 1000; iteration++)
            foreach (var entry in source) GC.KeepAlive(entry.Value.unwrap());
        var before = GC.GetAllocatedBytesForCurrentThread();
        var matches = 0;
        for (var iteration = 0; iteration < 1000; iteration++)
            foreach (var entry in source)
                if (entry.Key == "count" && ReferenceEquals(entry.Value.unwrap(), "kept")) matches++;
        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
        Assert.Equal(1000, matches);
    }

    [Fact]
    public void ClosedArrayProtocolRetainsSlotsBoundsAndNativeIdentity()
    {
        var source = new TsArray();
        IDynamicArray view = source;
        Assert.Same(source, view);
        Assert.True(view.TrySetAt(2, ulong.MaxValue));
        Assert.Equal(3, view.Length);
        Assert.False(view.HasIndex(0));
        Assert.False(view.TryGetAt(0, out var missing));
        Assert.Null(missing);
        Assert.True(view.TryGetAt(2, out var present));
        Assert.Equal(ulong.MaxValue, Assert.IsType<ulong>(present));
        Assert.True(view.HasOwn("length"));
        Assert.True(view.HasOwn("2"));
        Assert.False(view.HasOwn("unknown"));
        Assert.Single(view.Entries());
        Assert.True(view.TryReadDynamicSlot("2", out present));
        Assert.Equal(ulong.MaxValue, Assert.IsType<ulong>(present));
        Assert.False(view.TryReadDynamicSlot("unknown", out _));
        view.WriteDynamicSlot("1", null);
        Assert.True(view.HasIndex(1));
        Assert.True(view.TryGetAt(1, out present));
        Assert.Null(present);
        Assert.True(view.DeleteAt(2));
        Assert.False(view.HasIndex(2));
        Assert.Equal(1, view.SetLength(1));
        Assert.Empty(view.Entries());
        Assert.False(view.TrySetAt(-1, "invalid"));
        Assert.Throws<RangeError>(() => view.SetLength(-1));
        Assert.Equal(1, view.Length);
    }

    [Fact]
    public void ClosedArrayProtocolReadsHaveNoAdapterAllocation()
    {
        var source = new TsArray(new object?[] { "kept" });
        IDynamicArray view = source;
        for (var iteration = 0; iteration < 1000; iteration++) view.TryGetAt(0, out _);
        var before = GC.GetAllocatedBytesForCurrentThread();
        var matches = 0;
        for (var iteration = 0; iteration < 1000; iteration++)
            if (view.TryGetAt(0, out var value) && ReferenceEquals(value, "kept")) matches++;
        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
        Assert.Equal(1000, matches);
    }
}
