using System;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class RecordFieldTests
{
    private struct ValueRecord
    {
        internal RecordField<int> Count;
    }

    private sealed class ReferenceRecord
    {
        internal RecordField<int> Count;
        internal int Visits;
    }

    [Fact]
    public void BoundCopyAddressAndWriteNeverReadThePointer()
    {
        var reads = 0;
        var value = 3;
        var pointer = Location<int>.Bind(new object(), () => { reads++; return value; }, next => value = next);
        var record = new ValueRecord { Count = RecordField<int>.FromLocation(pointer) };
        var returned = Location<ValueRecord>.Allocate(record);
        var address = RecordField<int>.FromValueLocation(returned, "count", owner => owner.Count,
            (owner, next) => { owner.Count.Value = next; return owner; });
        Assert.Same(pointer, address);
        Assert.Equal(Location<int>.Hash(pointer), Location<int>.Hash(address));
        address.Store(7);
        Assert.Equal(0, reads);
        Assert.Equal(7, value);
        Assert.Equal(7, record.Count.Value);
        Assert.Equal(1, reads);
    }

    [Fact]
    public void OrdinaryCopiesAreIndependentAndAddressesRemainParentRelative()
    {
        var record = new ValueRecord { Count = RecordField<int>.FromValue(3) };
        var copy = record;
        copy.Count.Value = 5;
        Assert.Equal(3, record.Count.Value);
        var owner = Location<ValueRecord>.Allocate(record);
        var first = RecordField<int>.FromValueLocation(owner, "count", current => current.Count,
            (current, value) => { current.Count.Value = value; return current; });
        var alias = RecordField<int>.FromValueLocation(owner, "count", current => current.Count,
            (current, value) => { current.Count.Value = value; return current; });
        Assert.True(Location<int>.Same(first, alias));
        owner.Store(new ValueRecord { Count = RecordField<int>.FromValue(11) });
        Assert.Equal(11, first.Load());
        first.Store(13);
        Assert.Equal(13, owner.Load().Count.Value);
    }

    [Fact]
    public void ReferenceCallbacksCanAccessTheirRecordAndPreserveExceptions()
    {
        var record = new ReferenceRecord();
        var failure = new InvalidOperationException("selected failure");
        var pointer = Location<int>.Bind(new object(),
            () => { record.Visits++; throw failure; },
            _ => { record.Visits++; throw failure; });
        record.Count = RecordField<int>.FromLocation(pointer);
        var address = RecordField<int>.FromReference(record, "count", owner => owner.Count,
            (owner, next) => owner.Count.Value = next);
        Assert.Same(pointer, address);
        Assert.Equal(0, record.Visits);
        Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => record.Count.Value));
        Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => record.Count.Value = 5));
        Assert.Equal(2, record.Visits);
    }

    [Fact]
    public void ReplacingBoundStorageDoesNotRetargetExistingAddresses()
    {
        var first = Location<int>.Allocate(3);
        var second = Location<int>.Allocate(5);
        var record = new ReferenceRecord { Count = RecordField<int>.FromLocation(first) };
        var old = RecordField<int>.FromReference(record, "count", owner => owner.Count,
            (owner, value) => owner.Count.Value = value);
        record.Count = RecordField<int>.FromLocation(second);
        var next = RecordField<int>.FromReference(record, "count", owner => owner.Count,
            (owner, value) => owner.Count.Value = value);
        old.Store(7);
        next.Store(11);
        Assert.Equal(7, first.Load());
        Assert.Equal(11, second.Load());
        Assert.False(Location<int>.Same(old, next));
    }

    [Fact]
    public void NullSelectionsAreRejectedBeforeCreatingLocations()
    {
        Assert.Throws<ArgumentNullException>(() => RecordField<int>.FromLocation(null!));
        Assert.Throws<ArgumentNullException>(() => RecordField<int>.FromReference<ReferenceRecord>(null!, "count",
            owner => owner.Count, (owner, value) => owner.Count.Value = value));
        Assert.Throws<ArgumentNullException>(() => RecordField<int>.FromValueLocation<ValueRecord>(null!, "count",
            owner => owner.Count, (owner, value) => { owner.Count.Value = value; return owner; }));
    }
}
