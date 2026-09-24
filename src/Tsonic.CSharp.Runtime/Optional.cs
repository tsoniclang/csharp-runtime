using System;
using System.Collections.Generic;

namespace Tsonic.CSharp.Runtime;

public interface IAbsentValue<TSelf> where TSelf : IAbsentValue<TSelf>
{
    static abstract TSelf Singleton { get; }
}

public static class Optional
{
    public static Optional<T, TAbsent> FromNullable<T, TAbsent>(T? value)
        where T : struct where TAbsent : IAbsentValue<TAbsent> =>
        value.HasValue ? Optional<T, TAbsent>.From2(value.Value) : default;

    public static T? ToNullable<T, TAbsent>(Optional<T, TAbsent> value)
        where T : struct where TAbsent : IAbsentValue<TAbsent> =>
        value.Is2() ? value.As2() : null;

    public static Optional<T, TAbsent> FromReference<T, TAbsent>(T? value)
        where T : class where TAbsent : IAbsentValue<TAbsent> =>
        value is null ? default : Optional<T, TAbsent>.From2(value);

    public static T? ToReference<T, TAbsent>(Optional<T, TAbsent> value)
        where T : class where TAbsent : IAbsentValue<TAbsent> =>
        value.Is2() ? value.As2() : null;
}

public readonly struct Optional<T, TAbsent> : IEquatable<Optional<T, TAbsent>>
    where TAbsent : IAbsentValue<TAbsent>
{
    private readonly bool _present;
    private readonly T _value;

    private Optional(T value)
    {
        _present = true;
        _value = value;
    }

    public static Optional<T, TAbsent> From1(TAbsent value) => default;
    public static Optional<T, TAbsent> From2(T value) => new(value);
    public bool Is1() => !_present;
    public bool Is2() => _present;
    public TAbsent As1() => !_present ? TAbsent.Singleton : throw new InvalidOperationException("Optional contains a value.");
    public T As2() => _present ? _value : throw new InvalidOperationException("Optional is absent.");
    public static implicit operator Optional<T, TAbsent>(T value) => From2(value);
    public static implicit operator Optional<T, TAbsent>(TAbsent value) => default;

    public bool Equals(Optional<T, TAbsent> other) => _present == other._present &&
        (!_present || EqualityComparer<T>.Default.Equals(_value, other._value));
    public override bool Equals(object? value) => value is Optional<T, TAbsent> other && Equals(other);
    public override int GetHashCode() => _present ? HashCode.Combine(true, _value) : 0;
    public override string? ToString() => _present ? _value?.ToString() : TAbsent.Singleton.ToString();

    public static bool operator ==(Optional<T, TAbsent> left, Optional<T, TAbsent> right) => left.Equals(right);
    public static bool operator !=(Optional<T, TAbsent> left, Optional<T, TAbsent> right) => !left.Equals(right);
    public static bool operator ==(Optional<T, TAbsent> left, T right) => left._present && EqualityComparer<T>.Default.Equals(left._value, right);
    public static bool operator !=(Optional<T, TAbsent> left, T right) => !(left == right);
    public static bool operator ==(T left, Optional<T, TAbsent> right) => right == left;
    public static bool operator !=(T left, Optional<T, TAbsent> right) => !(right == left);
    public static bool operator ==(Optional<T, TAbsent> left, TAbsent right) => !left._present;
    public static bool operator !=(Optional<T, TAbsent> left, TAbsent right) => left._present;
    public static bool operator ==(TAbsent left, Optional<T, TAbsent> right) => !right._present;
    public static bool operator !=(TAbsent left, Optional<T, TAbsent> right) => right._present;
}
