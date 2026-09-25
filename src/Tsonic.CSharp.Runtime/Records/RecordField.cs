using System;

namespace Tsonic.CSharp.Runtime;

/// <summary>A record field containing an ordinary value or an explicitly bound location.</summary>
public struct RecordField<T>
{
    private T value;
    private readonly Location<T>? location;

    private RecordField(T value, Location<T>? location)
    {
        this.value = value;
        this.location = location;
    }

    /// <summary>Creates ordinary value storage without changing its copy semantics.</summary>
    public static RecordField<T> FromValue(T value) => new(value, null);

    /// <summary>Retains the selected location without reading its value.</summary>
    public static RecordField<T> FromLocation(Location<T> location)
    {
        ArgumentNullException.ThrowIfNull(location);
        return new(default!, location);
    }

    /// <summary>Returns the exact bound location, or null for ordinary value storage.</summary>
    public readonly Location<T>? BoundLocation => location;

    /// <summary>Returns a reference record member's original bound location or exact ordinary member location.</summary>
    public static Location<T> FromReference<TState>(TState state, string member,
        Func<TState, RecordField<T>> select, Action<TState, T> store) where TState : class
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(select);
        ArgumentNullException.ThrowIfNull(store);
        return select(state).BoundLocation ?? Location<T>.CreateMember(state, member,
            owner => select(owner).Value, store);
    }

    /// <summary>Returns a value record member's original bound location or exact parent-relative location.</summary>
    public static Location<T> FromValueLocation<TState>(Location<TState> state, string member,
        Func<TState, RecordField<T>> select, Func<TState, T, TState> store)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(select);
        ArgumentNullException.ThrowIfNull(store);
        return select(state.Load()).BoundLocation ?? state.ProjectMember(member,
            owner => select(owner).Value, store);
    }

    /// <summary>Reads or writes the selected storage, preserving bound callback exceptions.</summary>
    public T Value
    {
        readonly get => location is null ? value : location.Load();
        set
        {
            if (location is null) this.value = value;
            else location.Store(value);
        }
    }
}
