using System;

namespace Tsonic.CSharp.Runtime;

public interface IOptionalStorage<T, TStorage>
{
    static abstract TStorage From1(object? value);
    static abstract TStorage From2(T value);
    static abstract bool Is1(TStorage value);
    static abstract bool Is2(TStorage value);
    static abstract object? As1(TStorage value);
    static abstract T As2(TStorage value);
}

public readonly struct ValueOptionalStorage<T> : IOptionalStorage<T, T?> where T : struct
{
    public static T? From1(object? value) => null;
    public static T? From2(T value) => value;
    public static bool Is1(T? value) => !value.HasValue;
    public static bool Is2(T? value) => value.HasValue;
    public static object? As1(T? value) => !value.HasValue ? null : throw new InvalidOperationException("Optional contains a value.");
    public static T As2(T? value) => value ?? throw new InvalidOperationException("Optional is absent.");
}

public readonly struct NullableValueOptionalStorage<T> : IOptionalStorage<T?, T?> where T : struct
{
    public static T? From1(object? value) => null;
    public static T? From2(T? value) => value;
    public static bool Is1(T? value) => !value.HasValue;
    public static bool Is2(T? value) => value.HasValue;
    public static object? As1(T? value) => !value.HasValue ? null : throw new InvalidOperationException("Optional contains a value.");
    public static T? As2(T? value) => value.HasValue ? value : throw new InvalidOperationException("Optional is absent.");
}

public readonly struct ReferenceOptionalStorage<T> : IOptionalStorage<T, T?> where T : class?
{
    public static T? From1(object? value) => null;
    public static T? From2(T value) => value;
    public static bool Is1(T? value) => value is null;
    public static bool Is2(T? value) => value is not null;
    public static object? As1(T? value) => value is null ? null : throw new InvalidOperationException("Optional contains a value.");
    public static T As2(T? value) => value ?? throw new InvalidOperationException("Optional is absent.");
}

public readonly struct AbsentOptionalStorage : IOptionalStorage<object?, object?>
{
    public static object? From1(object? value) => null;
    public static object? From2(object? value) => null;
    public static bool Is1(object? value) => true;
    public static bool Is2(object? value) => false;
    public static object? As1(object? value) => null;
    public static object? As2(object? value) => throw new InvalidOperationException("Optional is absent.");
}

public readonly struct JsValueOptionalStorage : IOptionalStorage<TsValue, TsValue>
{
    public static TsValue From1(object? value) => TsValue.from(null);
    public static TsValue From2(TsValue value) => value;
    public static bool Is1(TsValue value) => value.isUndefined();
    public static bool Is2(TsValue value) => !value.isUndefined();
    public static object? As1(TsValue value) => value.isUndefined() ? null : throw new InvalidOperationException("Optional contains a value.");
    public static TsValue As2(TsValue value) => !value.isUndefined() ? value : throw new InvalidOperationException("Optional is absent.");
}
