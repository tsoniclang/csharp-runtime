using System;

namespace Tsonic.CSharp.Runtime;

public readonly struct SourceUnionRole1<TValue1, TValue2, TValue3, TValue4> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4>, TValue1>
{
    public static Union<TValue1, TValue2, TValue3, TValue4> From(TValue1 value) => Union<TValue1, TValue2, TValue3, TValue4>.From1(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4> value) => value.Is1();
    public static TValue1 As(Union<TValue1, TValue2, TValue3, TValue4> value) => value.As1();
}

public readonly struct SourceUnionRole2<TValue1, TValue2, TValue3, TValue4> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4>, TValue2>
{
    public static Union<TValue1, TValue2, TValue3, TValue4> From(TValue2 value) => Union<TValue1, TValue2, TValue3, TValue4>.From2(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4> value) => value.Is2();
    public static TValue2 As(Union<TValue1, TValue2, TValue3, TValue4> value) => value.As2();
}

public readonly struct SourceUnionRole3<TValue1, TValue2, TValue3, TValue4> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4>, TValue3>
{
    public static Union<TValue1, TValue2, TValue3, TValue4> From(TValue3 value) => Union<TValue1, TValue2, TValue3, TValue4>.From3(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4> value) => value.Is3();
    public static TValue3 As(Union<TValue1, TValue2, TValue3, TValue4> value) => value.As3();
}

public readonly struct SourceUnionRole4<TValue1, TValue2, TValue3, TValue4> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4>, TValue4>
{
    public static Union<TValue1, TValue2, TValue3, TValue4> From(TValue4 value) => Union<TValue1, TValue2, TValue3, TValue4>.From4(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4> value) => value.Is4();
    public static TValue4 As(Union<TValue1, TValue2, TValue3, TValue4> value) => value.As4();
}

public readonly struct SourceUnionValueCategory<TValue1, TCategory1, TValue2, TCategory2, TValue3, TCategory3, TValue4, TCategory4> : ISourceValueCategory<Union<TValue1, TValue2, TValue3, TValue4>>
    where TCategory1 : struct, ISourceValueCategory<TValue1>
    where TCategory2 : struct, ISourceValueCategory<TValue2>
    where TCategory3 : struct, ISourceValueCategory<TValue3>
    where TCategory4 : struct, ISourceValueCategory<TValue4>
{
    public static SourceValueCategory Category(Union<TValue1, TValue2, TValue3, TValue4> value) =>
        value.Is1() ? TCategory1.Category(value.As1()) :
        value.Is2() ? TCategory2.Category(value.As2()) :
        value.Is3() ? TCategory3.Category(value.As3()) :
        value.Is4() ? TCategory4.Category(value.As4()) :
        throw new InvalidOperationException("A Source union category requires an initialized native arm.");
}
