using System;
using System.Runtime.CompilerServices;

namespace Tsonic.CSharp.Runtime;

public readonly partial struct TsValue
{
    public static TsValue fromsource<TValue, TCategory>(TValue value)
        where TCategory : struct, ISourceValueCategory<TValue> => fromsource(value, TCategory.Category(value));

    public static TsValue fromsource<TValue>(TValue value, SourceValueCategory category) => SourceValue(value, category);

    public static TsValue fromnative<TValue>(TValue value)
    {
        if (typeof(TValue) == typeof(TsValue)) return Unsafe.As<TValue, TsValue>(ref value);
        object? payload = value;
        if (payload is TsValue stored) return stored;
        return new TsValue(payload, nativecategory(payload));
    }

    private static TsValue SourceValue<TValue>(TValue value, SourceValueCategory category)
    {
        if (category > SourceValueCategory.Function) throw new ArgumentOutOfRangeException(nameof(category));
        if (typeof(TValue) == typeof(TsValue)) return PreserveSourceValue(Unsafe.As<TValue, TsValue>(ref value), category);
        object? payload = value;
        if (payload is TsValue stored) return PreserveSourceValue(stored, category);
        if (payload is IClosedUnionValue { IsInitialized: false }) throw new InvalidOperationException("Union is not initialized.");
        if (category == SourceValueCategory.Absent && UnwrapClosedValue(payload) is null) return undefined();
        if (payload is null || category == SourceValueCategory.Absent)
            throw new ArgumentException("Source absence requires the one native absence value.", nameof(category));
        validateClosedValue(payload);
        return new TsValue(payload, category);
    }

    private static TsValue PreserveSourceValue(TsValue value, SourceValueCategory category) => value.Category == category
        ? value : throw new ArgumentException("Source erasure cannot replace an existing value category.", nameof(category));

}
