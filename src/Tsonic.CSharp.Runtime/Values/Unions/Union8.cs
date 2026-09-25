using System;
using System.Collections.Generic;

namespace Tsonic.CSharp.Runtime;

public readonly struct Union<T1, T2, T3, T4, T5, T6, T7, T8> : IEquatable<Union<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    private readonly byte _index;
    private readonly T1 _value1;
    private readonly T2 _value2;
    private readonly T3 _value3;
    private readonly T4 _value4;
    private readonly T5 _value5;
    private readonly T6 _value6;
    private readonly T7 _value7;
    private readonly T8 _value8;

    private Union(byte index, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
    {
        _index = index;
        _value1 = value1;
        _value2 = value2;
        _value3 = value3;
        _value4 = value4;
        _value5 = value5;
        _value6 = value6;
        _value7 = value7;
        _value8 = value8;
    }

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From1(T1 value) => new(1, value, default!, default!, default!, default!, default!, default!, default!);
    public bool Is1() => _index == 1;
    public T1 As1() => Is1() ? _value1 : throw new InvalidOperationException("Union does not contain arm 1.");
    public bool TryAs1(out T1? value)
    {
        value = Is1() ? _value1 : default;
        return Is1();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value) => From1(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From2(T2 value) => new(2, default!, value, default!, default!, default!, default!, default!, default!);
    public bool Is2() => _index == 2;
    public T2 As2() => Is2() ? _value2 : throw new InvalidOperationException("Union does not contain arm 2.");
    public bool TryAs2(out T2? value)
    {
        value = Is2() ? _value2 : default;
        return Is2();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T2 value) => From2(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From3(T3 value) => new(3, default!, default!, value, default!, default!, default!, default!, default!);
    public bool Is3() => _index == 3;
    public T3 As3() => Is3() ? _value3 : throw new InvalidOperationException("Union does not contain arm 3.");
    public bool TryAs3(out T3? value)
    {
        value = Is3() ? _value3 : default;
        return Is3();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T3 value) => From3(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From4(T4 value) => new(4, default!, default!, default!, value, default!, default!, default!, default!);
    public bool Is4() => _index == 4;
    public T4 As4() => Is4() ? _value4 : throw new InvalidOperationException("Union does not contain arm 4.");
    public bool TryAs4(out T4? value)
    {
        value = Is4() ? _value4 : default;
        return Is4();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T4 value) => From4(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From5(T5 value) => new(5, default!, default!, default!, default!, value, default!, default!, default!);
    public bool Is5() => _index == 5;
    public T5 As5() => Is5() ? _value5 : throw new InvalidOperationException("Union does not contain arm 5.");
    public bool TryAs5(out T5? value)
    {
        value = Is5() ? _value5 : default;
        return Is5();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T5 value) => From5(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From6(T6 value) => new(6, default!, default!, default!, default!, default!, value, default!, default!);
    public bool Is6() => _index == 6;
    public T6 As6() => Is6() ? _value6 : throw new InvalidOperationException("Union does not contain arm 6.");
    public bool TryAs6(out T6? value)
    {
        value = Is6() ? _value6 : default;
        return Is6();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T6 value) => From6(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From7(T7 value) => new(7, default!, default!, default!, default!, default!, default!, value, default!);
    public bool Is7() => _index == 7;
    public T7 As7() => Is7() ? _value7 : throw new InvalidOperationException("Union does not contain arm 7.");
    public bool TryAs7(out T7? value)
    {
        value = Is7() ? _value7 : default;
        return Is7();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T7 value) => From7(value);

    public static Union<T1, T2, T3, T4, T5, T6, T7, T8> From8(T8 value) => new(8, default!, default!, default!, default!, default!, default!, default!, value);
    public bool Is8() => _index == 8;
    public T8 As8() => Is8() ? _value8 : throw new InvalidOperationException("Union does not contain arm 8.");
    public bool TryAs8(out T8? value)
    {
        value = Is8() ? _value8 : default;
        return Is8();
    }
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T8 value) => From8(value);

    public TResult AsReference<TResult>() where TResult : class => _index switch
    {
        1 => (TResult)(object)_value1!,
        2 => (TResult)(object)_value2!,
        3 => (TResult)(object)_value3!,
        4 => (TResult)(object)_value4!,
        5 => (TResult)(object)_value5!,
        6 => (TResult)(object)_value6!,
        7 => (TResult)(object)_value7!,
        8 => (TResult)(object)_value8!,
        _ => throw new InvalidOperationException("Union is not initialized."),
    };

    public TResult Match<TResult>(Func<T1, TResult> onT1, Func<T2, TResult> onT2, Func<T3, TResult> onT3, Func<T4, TResult> onT4, Func<T5, TResult> onT5, Func<T6, TResult> onT6, Func<T7, TResult> onT7, Func<T8, TResult> onT8) => _index switch
    {
        1 => onT1(_value1),
        2 => onT2(_value2),
        3 => onT3(_value3),
        4 => onT4(_value4),
        5 => onT5(_value5),
        6 => onT6(_value6),
        7 => onT7(_value7),
        8 => onT8(_value8),
        _ => throw new InvalidOperationException("Union is not initialized."),
    };

    public void Match(Action<T1> onT1, Action<T2> onT2, Action<T3> onT3, Action<T4> onT4, Action<T5> onT5, Action<T6> onT6, Action<T7> onT7, Action<T8> onT8)
    {
        switch (_index)
        {
            case 1: onT1(_value1); break;
            case 2: onT2(_value2); break;
            case 3: onT3(_value3); break;
            case 4: onT4(_value4); break;
            case 5: onT5(_value5); break;
            case 6: onT6(_value6); break;
            case 7: onT7(_value7); break;
            case 8: onT8(_value8); break;
            default: throw new InvalidOperationException("Union is not initialized.");
        }
    }

    public override string? ToString() => _index switch
    {
        1 => _value1?.ToString(),
        2 => _value2?.ToString(),
        3 => _value3?.ToString(),
        4 => _value4?.ToString(),
        5 => _value5?.ToString(),
        6 => _value6?.ToString(),
        7 => _value7?.ToString(),
        8 => _value8?.ToString(),
        _ => null,
    };

    public bool Equals(Union<T1, T2, T3, T4, T5, T6, T7, T8> other) => _index == other._index && (_index switch
    {
        1 => EqualityComparer<T1>.Default.Equals(_value1, other._value1),
        2 => EqualityComparer<T2>.Default.Equals(_value2, other._value2),
        3 => EqualityComparer<T3>.Default.Equals(_value3, other._value3),
        4 => EqualityComparer<T4>.Default.Equals(_value4, other._value4),
        5 => EqualityComparer<T5>.Default.Equals(_value5, other._value5),
        6 => EqualityComparer<T6>.Default.Equals(_value6, other._value6),
        7 => EqualityComparer<T7>.Default.Equals(_value7, other._value7),
        8 => EqualityComparer<T8>.Default.Equals(_value8, other._value8),
        _ => true,
    });
    public override bool Equals(object? value) => value is Union<T1, T2, T3, T4, T5, T6, T7, T8> other && Equals(other);
    public override int GetHashCode() => _index switch
    {
        1 => HashCode.Combine(_index, _value1),
        2 => HashCode.Combine(_index, _value2),
        3 => HashCode.Combine(_index, _value3),
        4 => HashCode.Combine(_index, _value4),
        5 => HashCode.Combine(_index, _value5),
        6 => HashCode.Combine(_index, _value6),
        7 => HashCode.Combine(_index, _value7),
        8 => HashCode.Combine(_index, _value8),
        _ => 0,
    };

    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T1 right) => left.HasValue && left.Value.Is1() && EqualityComparer<T1>.Default.Equals(left.Value._value1, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T1 right) => !(left == right);
    public static bool operator ==(T1 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T1 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T2 right) => left.HasValue && left.Value.Is2() && EqualityComparer<T2>.Default.Equals(left.Value._value2, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T2 right) => !(left == right);
    public static bool operator ==(T2 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T2 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T3 right) => left.HasValue && left.Value.Is3() && EqualityComparer<T3>.Default.Equals(left.Value._value3, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T3 right) => !(left == right);
    public static bool operator ==(T3 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T3 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T4 right) => left.HasValue && left.Value.Is4() && EqualityComparer<T4>.Default.Equals(left.Value._value4, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T4 right) => !(left == right);
    public static bool operator ==(T4 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T4 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T5 right) => left.HasValue && left.Value.Is5() && EqualityComparer<T5>.Default.Equals(left.Value._value5, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T5 right) => !(left == right);
    public static bool operator ==(T5 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T5 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T6 right) => left.HasValue && left.Value.Is6() && EqualityComparer<T6>.Default.Equals(left.Value._value6, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T6 right) => !(left == right);
    public static bool operator ==(T6 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T6 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T7 right) => left.HasValue && left.Value.Is7() && EqualityComparer<T7>.Default.Equals(left.Value._value7, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T7 right) => !(left == right);
    public static bool operator ==(T7 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T7 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
    public static bool operator ==(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T8 right) => left.HasValue && left.Value.Is8() && EqualityComparer<T8>.Default.Equals(left.Value._value8, right);
    public static bool operator !=(Union<T1, T2, T3, T4, T5, T6, T7, T8>? left, T8 right) => !(left == right);
    public static bool operator ==(T8 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => right == left;
    public static bool operator !=(T8 left, Union<T1, T2, T3, T4, T5, T6, T7, T8>? right) => !(right == left);
}
