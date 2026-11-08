using System;

namespace Tsonic.CSharp.Runtime;

public readonly struct SourceUnionRole1<TValue1, TValue2> : ISourceUnionRole<Union<TValue1, TValue2>, TValue1>
{
    public static Union<TValue1, TValue2> From(TValue1 value) => Union<TValue1, TValue2>.From1(value);
    public static bool Is(Union<TValue1, TValue2> value) => value.Is1();
    public static TValue1 As(Union<TValue1, TValue2> value) => value.As1();
}

public readonly struct SourceUnionRole2<TValue1, TValue2> : ISourceUnionRole<Union<TValue1, TValue2>, TValue2>
{
    public static Union<TValue1, TValue2> From(TValue2 value) => Union<TValue1, TValue2>.From2(value);
    public static bool Is(Union<TValue1, TValue2> value) => value.Is2();
    public static TValue2 As(Union<TValue1, TValue2> value) => value.As2();
}

public readonly struct SourceUnionValueCategory<TValue1, TCategory1, TValue2, TCategory2> : ISourceValueCategory<Union<TValue1, TValue2>>
    where TCategory1 : struct, ISourceValueCategory<TValue1>
    where TCategory2 : struct, ISourceValueCategory<TValue2>
{
    public static SourceValueCategory Category(Union<TValue1, TValue2> value) =>
        value.Is1() ? TCategory1.Category(value.As1()) :
        value.Is2() ? TCategory2.Category(value.As2()) :
        throw new InvalidOperationException("A Source union category requires an initialized native arm.");
}
