using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tsonic.CSharp.Runtime;

public static class RecordOperations
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue GetOrDefault<TKey, TValue>(Dictionary<TKey, TValue> record, TKey key)
        where TKey : notnull
        => record.TryGetValue(key, out var value) ? value : default!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<TKey, TValue> Set<TKey, TValue>(Dictionary<TKey, TValue> record, TKey key, TValue value)
        where TKey : notnull
    {
        record[key] = value;
        return record;
    }

    public static Dictionary<TKey, TValue> Extend<TKey, TValue>(Dictionary<TKey, TValue> record, Dictionary<TKey, TValue> source)
        where TKey : notnull
    {
        if (ReferenceEquals(record, source)) return record;
        foreach (var pair in source) record[pair.Key] = pair.Value;
        return record;
    }
}
