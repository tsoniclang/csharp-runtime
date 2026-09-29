using System.Collections.Generic;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class RecordOperationsTests
{
    [Fact]
    public void OptionalReadsAndWritesUseOneNativeDictionary()
    {
        var record = new Dictionary<string, ulong?>();
        Assert.Null(RecordOperations.GetOrDefault(record, "absent"));
        Assert.Same(record, RecordOperations.Set(record, "exact", 9_007_199_254_740_993UL));
        Assert.Equal(9_007_199_254_740_993UL, RecordOperations.GetOrDefault(record, "exact"));
        Assert.Same(record, RecordOperations.Set(record, "empty", null));
        Assert.True(record.ContainsKey("empty"));
        Assert.Null(RecordOperations.GetOrDefault(record, "empty"));
        Assert.Throws<KeyNotFoundException>(() => record["absent"]);
    }

    [Fact]
    public void SpreadAndAssignmentPreserveEvaluationAndIdentity()
    {
        var value = new object();
        var source = new Dictionary<string, object> { ["key"] = value };
        var target = new Dictionary<string, object>();
        Assert.Same(target, RecordOperations.Extend(target, source));
        Assert.Same(value, target["key"]);
        var replacement = new object();
        Assert.Same(target, RecordOperations.Set(target, "key", replacement));
        Assert.Same(value, source["key"]);
        Assert.Same(replacement, target["key"]);
        Assert.Same(target, RecordOperations.Extend(target, target));
    }
}
