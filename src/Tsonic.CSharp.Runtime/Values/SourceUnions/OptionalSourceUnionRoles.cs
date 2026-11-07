namespace Tsonic.CSharp.Runtime;

public readonly struct OptionalStorageSourceUnionRole<TStorage, TPresent, TRole, TOptional, TInner> : ISourceUnionRole<TStorage, TRole>
    where TOptional : struct, IOptionalStorage<TPresent, TStorage>
    where TInner : struct, ISourceUnionRole<TPresent, TRole>
{
    public static TStorage From(TRole value) => TOptional.From2(TInner.From(value));
    public static bool Is(TStorage value) => TOptional.Is2(value) && TInner.Is(TOptional.As2(value));
    public static TRole As(TStorage value) => TInner.As(TOptional.As2(value));
}

public readonly struct OptionalSourceUnionRole<TStorage, TPresent, TRoleStorage, TRolePresent, TStorageOptional, TRoleOptional, TInner> : ISourceUnionRole<TStorage, TRoleStorage>
    where TStorageOptional : struct, IOptionalStorage<TPresent, TStorage>
    where TRoleOptional : struct, IOptionalStorage<TRolePresent, TRoleStorage>
    where TInner : struct, ISourceUnionRole<TPresent, TRolePresent>
{
    public static TStorage From(TRoleStorage value) => TRoleOptional.Is1(value)
        ? TStorageOptional.From1(null)
        : TStorageOptional.From2(TInner.From(TRoleOptional.As2(value)));
    public static bool Is(TStorage value) => TStorageOptional.Is1(value) || TInner.Is(TStorageOptional.As2(value));
    public static TRoleStorage As(TStorage value) => TStorageOptional.Is1(value)
        ? TRoleOptional.From1(null)
        : TRoleOptional.From2(TInner.As(TStorageOptional.As2(value)));
}

public readonly struct AbsentSourceUnionRole<TStorage, TPresent, TOptional> : ISourceUnionRole<TStorage, object?>
    where TOptional : struct, IOptionalStorage<TPresent, TStorage>
{
    public static TStorage From(object? value) => TOptional.From1(value);
    public static bool Is(TStorage value) => TOptional.Is1(value);
    public static object? As(TStorage value) => TOptional.As1(value);
}
