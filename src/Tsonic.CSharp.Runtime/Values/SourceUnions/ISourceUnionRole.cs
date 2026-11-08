using System;

namespace Tsonic.CSharp.Runtime;

public interface ISourceUnionRole<TStorage, TRole>
{
    static abstract TStorage From(TRole value);
    static abstract bool Is(TStorage value);
    static abstract TRole As(TStorage value);
}

public readonly struct SourceUnionIdentity<TValue> : ISourceUnionRole<TValue, TValue>
{
    public static TValue From(TValue value) => value;
    public static bool Is(TValue value) => true;
    public static TValue As(TValue value) => value;
}

public readonly struct SourceUnionComposedRole<TStorage, TIntermediate, TRole, TOuter, TInner> : ISourceUnionRole<TStorage, TRole>
    where TOuter : struct, ISourceUnionRole<TStorage, TIntermediate>
    where TInner : struct, ISourceUnionRole<TIntermediate, TRole>
{
    public static TStorage From(TRole value) => TOuter.From(TInner.From(value));
    public static bool Is(TStorage value) => TOuter.Is(value) && TInner.Is(TOuter.As(value));
    public static TRole As(TStorage value) => TInner.As(TOuter.As(value));
}

public readonly struct SourceUnionOptionalRole<TPayload, TStorage, TRole, TOps, TInner> : ISourceUnionRole<TStorage, TRole>
    where TOps : struct, IOptionalStorage<TPayload, TStorage>
    where TInner : struct, ISourceUnionRole<TPayload, TRole>
{
    public static TStorage From(TRole value) => TOps.From2(TInner.From(value));
    public static bool Is(TStorage value) => TOps.Is2(value) && TInner.Is(TOps.As2(value));
    public static TRole As(TStorage value) => TInner.As(TOps.As2(value));
}

public readonly struct SourceUnionOptionalAbsence<TPayload, TStorage, TOps> : ISourceUnionRole<TStorage, object?>
    where TOps : struct, IOptionalStorage<TPayload, TStorage>
{
    public static TStorage From(object? value) => value is null ? TOps.From1(null)
        : throw new ArgumentException("An absence role requires the native absence value.", nameof(value));
    public static bool Is(TStorage value) => TOps.Is1(value);
    public static object? As(TStorage value) => TOps.As1(value);
}
