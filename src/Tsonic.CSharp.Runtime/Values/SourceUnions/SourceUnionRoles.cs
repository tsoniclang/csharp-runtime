using System;

namespace Tsonic.CSharp.Runtime;

public interface ISourceUnionRole<TStorage, TRole>
{
    static abstract TStorage From(TRole value);
    static abstract bool Is(TStorage value);
    static abstract TRole As(TStorage value);
}

public readonly struct IdentitySourceUnionRole<T> : ISourceUnionRole<T, T>
{
    public static T From(T value) => value;
    public static bool Is(T value) => true;
    public static T As(T value) => value;
}

public readonly struct SourceUnionArm1Of2<TArm1, TArm2> : ISourceUnionRole<Union<TArm1, TArm2>, TArm1>
{
    public static Union<TArm1, TArm2> From(TArm1 value) => Union<TArm1, TArm2>.From1(value);
    public static bool Is(Union<TArm1, TArm2> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2> value) => value.As1();
}

public readonly struct SourceUnionArm2Of2<TArm1, TArm2> : ISourceUnionRole<Union<TArm1, TArm2>, TArm2>
{
    public static Union<TArm1, TArm2> From(TArm2 value) => Union<TArm1, TArm2>.From2(value);
    public static bool Is(Union<TArm1, TArm2> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2> value) => value.As2();
}

public readonly struct SourceUnionArm1Of3<TArm1, TArm2, TArm3> : ISourceUnionRole<Union<TArm1, TArm2, TArm3>, TArm1>
{
    public static Union<TArm1, TArm2, TArm3> From(TArm1 value) => Union<TArm1, TArm2, TArm3>.From1(value);
    public static bool Is(Union<TArm1, TArm2, TArm3> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2, TArm3> value) => value.As1();
}

public readonly struct SourceUnionArm2Of3<TArm1, TArm2, TArm3> : ISourceUnionRole<Union<TArm1, TArm2, TArm3>, TArm2>
{
    public static Union<TArm1, TArm2, TArm3> From(TArm2 value) => Union<TArm1, TArm2, TArm3>.From2(value);
    public static bool Is(Union<TArm1, TArm2, TArm3> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2, TArm3> value) => value.As2();
}

public readonly struct SourceUnionArm3Of3<TArm1, TArm2, TArm3> : ISourceUnionRole<Union<TArm1, TArm2, TArm3>, TArm3>
{
    public static Union<TArm1, TArm2, TArm3> From(TArm3 value) => Union<TArm1, TArm2, TArm3>.From3(value);
    public static bool Is(Union<TArm1, TArm2, TArm3> value) => value.Is3();
    public static TArm3 As(Union<TArm1, TArm2, TArm3> value) => value.As3();
}

public readonly struct SourceUnionArm1Of4<TArm1, TArm2, TArm3, TArm4> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4>, TArm1>
{
    public static Union<TArm1, TArm2, TArm3, TArm4> From(TArm1 value) => Union<TArm1, TArm2, TArm3, TArm4>.From1(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2, TArm3, TArm4> value) => value.As1();
}

public readonly struct SourceUnionArm2Of4<TArm1, TArm2, TArm3, TArm4> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4>, TArm2>
{
    public static Union<TArm1, TArm2, TArm3, TArm4> From(TArm2 value) => Union<TArm1, TArm2, TArm3, TArm4>.From2(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2, TArm3, TArm4> value) => value.As2();
}

public readonly struct SourceUnionArm3Of4<TArm1, TArm2, TArm3, TArm4> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4>, TArm3>
{
    public static Union<TArm1, TArm2, TArm3, TArm4> From(TArm3 value) => Union<TArm1, TArm2, TArm3, TArm4>.From3(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4> value) => value.Is3();
    public static TArm3 As(Union<TArm1, TArm2, TArm3, TArm4> value) => value.As3();
}

public readonly struct SourceUnionArm4Of4<TArm1, TArm2, TArm3, TArm4> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4>, TArm4>
{
    public static Union<TArm1, TArm2, TArm3, TArm4> From(TArm4 value) => Union<TArm1, TArm2, TArm3, TArm4>.From4(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4> value) => value.Is4();
    public static TArm4 As(Union<TArm1, TArm2, TArm3, TArm4> value) => value.As4();
}

public readonly struct SourceUnionArm1Of5<TArm1, TArm2, TArm3, TArm4, TArm5> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5>, TArm1>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5> From(TArm1 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From1(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.As1();
}

public readonly struct SourceUnionArm2Of5<TArm1, TArm2, TArm3, TArm4, TArm5> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5>, TArm2>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5> From(TArm2 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From2(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.As2();
}

public readonly struct SourceUnionArm3Of5<TArm1, TArm2, TArm3, TArm4, TArm5> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5>, TArm3>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5> From(TArm3 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From3(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.Is3();
    public static TArm3 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.As3();
}

public readonly struct SourceUnionArm4Of5<TArm1, TArm2, TArm3, TArm4, TArm5> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5>, TArm4>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5> From(TArm4 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From4(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.Is4();
    public static TArm4 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.As4();
}

public readonly struct SourceUnionArm5Of5<TArm1, TArm2, TArm3, TArm4, TArm5> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5>, TArm5>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5> From(TArm5 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From5(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.Is5();
    public static TArm5 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value) => value.As5();
}

public readonly struct SourceUnionArm1Of6<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>, TArm1>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> From(TArm1 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From1(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.As1();
}

public readonly struct SourceUnionArm2Of6<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>, TArm2>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> From(TArm2 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From2(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.As2();
}

public readonly struct SourceUnionArm3Of6<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>, TArm3>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> From(TArm3 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From3(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.Is3();
    public static TArm3 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.As3();
}

public readonly struct SourceUnionArm4Of6<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>, TArm4>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> From(TArm4 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From4(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.Is4();
    public static TArm4 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.As4();
}

public readonly struct SourceUnionArm5Of6<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>, TArm5>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> From(TArm5 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From5(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.Is5();
    public static TArm5 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.As5();
}

public readonly struct SourceUnionArm6Of6<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>, TArm6>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> From(TArm6 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From6(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.Is6();
    public static TArm6 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value) => value.As6();
}

public readonly struct SourceUnionArm1Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm1>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm1 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From1(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As1();
}

public readonly struct SourceUnionArm2Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm2>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm2 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From2(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As2();
}

public readonly struct SourceUnionArm3Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm3>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm3 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From3(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is3();
    public static TArm3 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As3();
}

public readonly struct SourceUnionArm4Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm4>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm4 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From4(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is4();
    public static TArm4 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As4();
}

public readonly struct SourceUnionArm5Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm5>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm5 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From5(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is5();
    public static TArm5 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As5();
}

public readonly struct SourceUnionArm6Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm6>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm6 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From6(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is6();
    public static TArm6 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As6();
}

public readonly struct SourceUnionArm7Of7<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>, TArm7>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> From(TArm7 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From7(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.Is7();
    public static TArm7 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value) => value.As7();
}

public readonly struct SourceUnionArm1Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm1>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm1 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From1(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is1();
    public static TArm1 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As1();
}

public readonly struct SourceUnionArm2Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm2>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm2 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From2(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is2();
    public static TArm2 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As2();
}

public readonly struct SourceUnionArm3Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm3>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm3 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From3(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is3();
    public static TArm3 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As3();
}

public readonly struct SourceUnionArm4Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm4>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm4 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From4(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is4();
    public static TArm4 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As4();
}

public readonly struct SourceUnionArm5Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm5>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm5 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From5(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is5();
    public static TArm5 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As5();
}

public readonly struct SourceUnionArm6Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm6>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm6 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From6(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is6();
    public static TArm6 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As6();
}

public readonly struct SourceUnionArm7Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm7>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm7 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From7(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is7();
    public static TArm7 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As7();
}

public readonly struct SourceUnionArm8Of8<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> : ISourceUnionRole<Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>, TArm8>
{
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> From(TArm8 value) => Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From8(value);
    public static bool Is(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.Is8();
    public static TArm8 As(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value) => value.As8();
}
