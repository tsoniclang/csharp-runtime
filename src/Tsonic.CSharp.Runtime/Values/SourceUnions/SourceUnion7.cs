using System;

namespace Tsonic.CSharp.Runtime;

public readonly struct SourceUnionRole1<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue1>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue1 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From1(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is1();
    public static TValue1 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As1();
}

public readonly struct SourceUnionRole2<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue2>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue2 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From2(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is2();
    public static TValue2 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As2();
}

public readonly struct SourceUnionRole3<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue3>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue3 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From3(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is3();
    public static TValue3 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As3();
}

public readonly struct SourceUnionRole4<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue4>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue4 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From4(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is4();
    public static TValue4 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As4();
}

public readonly struct SourceUnionRole5<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue5>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue5 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From5(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is5();
    public static TValue5 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As5();
}

public readonly struct SourceUnionRole6<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue6>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue6 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From6(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is6();
    public static TValue6 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As6();
}

public readonly struct SourceUnionRole7<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> : ISourceUnionRole<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>, TValue7>
{
    public static Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> From(TValue7 value) => Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>.From7(value);
    public static bool Is(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.Is7();
    public static TValue7 As(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) => value.As7();
}

public readonly struct SourceUnionValueCategory<TValue1, TCategory1, TValue2, TCategory2, TValue3, TCategory3, TValue4, TCategory4, TValue5, TCategory5, TValue6, TCategory6, TValue7, TCategory7> : ISourceValueCategory<Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>
    where TCategory1 : struct, ISourceValueCategory<TValue1>
    where TCategory2 : struct, ISourceValueCategory<TValue2>
    where TCategory3 : struct, ISourceValueCategory<TValue3>
    where TCategory4 : struct, ISourceValueCategory<TValue4>
    where TCategory5 : struct, ISourceValueCategory<TValue5>
    where TCategory6 : struct, ISourceValueCategory<TValue6>
    where TCategory7 : struct, ISourceValueCategory<TValue7>
{
    public static SourceValueCategory Category(Union<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> value) =>
        value.Is1() ? TCategory1.Category(value.As1()) :
        value.Is2() ? TCategory2.Category(value.As2()) :
        value.Is3() ? TCategory3.Category(value.As3()) :
        value.Is4() ? TCategory4.Category(value.As4()) :
        value.Is5() ? TCategory5.Category(value.As5()) :
        value.Is6() ? TCategory6.Category(value.As6()) :
        value.Is7() ? TCategory7.Category(value.As7()) :
        throw new InvalidOperationException("A Source union category requires an initialized native arm.");
}
