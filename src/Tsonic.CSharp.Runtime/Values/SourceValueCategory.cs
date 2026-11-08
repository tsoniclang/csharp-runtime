namespace Tsonic.CSharp.Runtime;

public enum SourceValueCategory : byte
{
    Absent,
    Boolean,
    Number,
    BigInt,
    String,
    Symbol,
    Object,
    Function,
}

public interface ISourceValueCategory<TValue>
{
    static abstract SourceValueCategory Category(TValue value);
}

public readonly struct SourceAbsentCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => SourceValueCategory.Absent;
}

public readonly struct SourceBooleanCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.Boolean;
}

public readonly struct SourceNumberCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.Number;
}

public readonly struct SourceBigIntCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.BigInt;
}

public readonly struct SourceStringCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.String;
}

public readonly struct SourceSymbolCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.Symbol;
}

public readonly struct SourceObjectCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.Object;
}

public readonly struct SourceFunctionCategory<TValue> : ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TValue value) => value is null ? SourceValueCategory.Absent : SourceValueCategory.Function;
}

public readonly struct SourceStoredValueCategory : ISourceValueCategory<TsValue>
{
    public static SourceValueCategory Category(TsValue value) => value.Category;
}

public readonly struct SourceOptionalValueCategory<TValue, TStorage, TOps, TCategory> : ISourceValueCategory<TStorage>
    where TOps : struct, IOptionalStorage<TValue, TStorage>
    where TCategory : struct, ISourceValueCategory<TValue>
{
    public static SourceValueCategory Category(TStorage value) => TOps.Is2(value)
        ? TCategory.Category(TOps.As2(value)) : SourceValueCategory.Absent;
}
