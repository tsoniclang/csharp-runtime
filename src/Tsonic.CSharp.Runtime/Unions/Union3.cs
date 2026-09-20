using System;
using System.Collections.Generic;

namespace Tsonic.CSharp.Runtime;

public readonly struct Union<T1, T2, T3> : IEquatable<Union<T1, T2, T3>>
{
    private readonly byte _index;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;

    private Union(byte index, T1 value1, T2 value2, T3 value3)
    {
        _index = index;
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
    }

    public static Union<T1, T2, T3> From1(T1 value) => new(1, value, default!, default!);
    public bool Is1() => _index == 1;
    public T1 As1() => Is1() ? _value1 : throw new InvalidOperationException("Union does not contain arm 1.");
    public bool TryAs1(out T1? value)
    {
        value = Is1() ? _value1 : default;
        return Is1();
    }
    public static implicit operator Union<T1, T2, T3>(T1 value) => From1(value);

    public static Union<T1, T2, T3> From2(T2 value) => new(2, default!, value, default!);
    public bool Is2() => _index == 2;
    public T2 As2() => Is2() ? _value2 : throw new InvalidOperationException("Union does not contain arm 2.");
    public bool TryAs2(out T2? value)
    {
        value = Is2() ? _value2 : default;
        return Is2();
    }
    public static implicit operator Union<T1, T2, T3>(T2 value) => From2(value);

    public static Union<T1, T2, T3> From3(T3 value) => new(3, default!, default!, value);
    public bool Is3() => _index == 3;
    public T3 As3() => Is3() ? _value3 : throw new InvalidOperationException("Union does not contain arm 3.");
    public bool TryAs3(out T3? value)
    {
        value = Is3() ? _value3 : default;
        return Is3();
    }
    public static implicit operator Union<T1, T2, T3>(T3 value) => From3(value);

    public TResult AsReference<TResult>() where TResult : class => _index switch
    {
        1 => (TResult)(object)_value1!,
        2 => (TResult)(object)_value2!,
        3 => (TResult)(object)_value3!,
        _ => throw new InvalidOperationException("Union is not initialized."),
    };

    public TResult Match<TResult>(Func<T1, TResult> onT1, Func<T2, TResult> onT2, Func<T3, TResult> onT3) => _index switch
    {
        1 => onT1(_value1),
        2 => onT2(_value2),
        3 => onT3(_value3),
        _ => throw new InvalidOperationException("Union is not initialized."),
    };

    public void Match(Action<T1> onT1, Action<T2> onT2, Action<T3> onT3)
    {
        switch (_index)
        {
            case 1: onT1(_value1); break;
            case 2: onT2(_value2); break;
            case 3: onT3(_value3); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    public override string? ToString() => _index switch
    {
        1 => _value1?.ToString(),
        2 => _value2?.ToString(),
        3 => _value3?.ToString(),
        _ => null,
    };

    public bool Equals(Union<T1, T2, T3> other) => _index == other._index && (_index switch
    {
        1 => EqualityComparer<T1>.Default.Equals(_value1, other._value1),
        2 => EqualityComparer<T2>.Default.Equals(_value2, other._value2),
        3 => EqualityComparer<T3>.Default.Equals(_value3, other._value3),
        _ => true,
    });
    public override bool Equals(object? value) => value is Union<T1, T2, T3> other && Equals(other);
    public override int GetHashCode() => _index switch
    {
        1 => HashCode.Combine(_index, _value1),
        2 => HashCode.Combine(_index, _value2),
        3 => HashCode.Combine(_index, _value3),
        _ => 0,
    };

    public static bool operator ==(Union<T1, T2, T3>? left, T1 right) => left.HasValue && left.Value.Is1() && EqualityComparer<T1>.Default.Equals(left.Value._value1, right);
    public static bool operator !=(Union<T1, T2, T3>? left, T1 right) => !(left == right);
    public static bool operator ==(T1 left, Union<T1, T2, T3>? right) => right == left;
    public static bool operator !=(T1 left, Union<T1, T2, T3>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3>? left, T2 right) => left.HasValue && left.Value.Is2() && EqualityComparer<T2>.Default.Equals(left.Value._value2, right);
    public static bool operator !=(Union<T1, T2, T3>? left, T2 right) => !(left == right);
    public static bool operator ==(T2 left, Union<T1, T2, T3>? right) => right == left;
    public static bool operator !=(T2 left, Union<T1, T2, T3>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3>? left, T3 right) => left.HasValue && left.Value.Is3() && EqualityComparer<T3>.Default.Equals(left.Value._value3, right);
    public static bool operator !=(Union<T1, T2, T3>? left, T3 right) => !(left == right);
    public static bool operator ==(T3 left, Union<T1, T2, T3>? right) => right == left;
    public static bool operator !=(T3 left, Union<T1, T2, T3>? right) => !(right == left);
}
