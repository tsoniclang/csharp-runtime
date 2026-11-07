using System;

namespace Tsonic.CSharp.Runtime;

public readonly struct CompositeSourceUnionRole2<TStorage, TArm1, TArm2, TRole1, TRole2> : ISourceUnionRole<TStorage, Union<TArm1, TArm2>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
{
    public static TStorage From(Union<TArm1, TArm2> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        return TRole2.From(value.As2());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value);
    public static Union<TArm1, TArm2> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2>.From1(TRole1.As(value));
        return Union<TArm1, TArm2>.From2(TRole2.As(value));
    }
}

public readonly struct CompositeSourceUnionRole3<TStorage, TArm1, TArm2, TArm3, TRole1, TRole2, TRole3> : ISourceUnionRole<TStorage, Union<TArm1, TArm2, TArm3>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
{
    public static TStorage From(Union<TArm1, TArm2, TArm3> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        if (value.Is2()) return TRole2.From(value.As2());
        return TRole3.From(value.As3());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value) || TRole3.Is(value);
    public static Union<TArm1, TArm2, TArm3> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2, TArm3>.From1(TRole1.As(value));
        if (TRole2.Is(value)) return Union<TArm1, TArm2, TArm3>.From2(TRole2.As(value));
        return Union<TArm1, TArm2, TArm3>.From3(TRole3.As(value));
    }
}

public readonly struct CompositeSourceUnionRole4<TStorage, TArm1, TArm2, TArm3, TArm4, TRole1, TRole2, TRole3, TRole4> : ISourceUnionRole<TStorage, Union<TArm1, TArm2, TArm3, TArm4>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
{
    public static TStorage From(Union<TArm1, TArm2, TArm3, TArm4> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        if (value.Is2()) return TRole2.From(value.As2());
        if (value.Is3()) return TRole3.From(value.As3());
        return TRole4.From(value.As4());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value) || TRole3.Is(value) || TRole4.Is(value);
    public static Union<TArm1, TArm2, TArm3, TArm4> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4>.From1(TRole1.As(value));
        if (TRole2.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4>.From2(TRole2.As(value));
        if (TRole3.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4>.From3(TRole3.As(value));
        return Union<TArm1, TArm2, TArm3, TArm4>.From4(TRole4.As(value));
    }
}

public readonly struct CompositeSourceUnionRole5<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TRole1, TRole2, TRole3, TRole4, TRole5> : ISourceUnionRole<TStorage, Union<TArm1, TArm2, TArm3, TArm4, TArm5>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
{
    public static TStorage From(Union<TArm1, TArm2, TArm3, TArm4, TArm5> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        if (value.Is2()) return TRole2.From(value.As2());
        if (value.Is3()) return TRole3.From(value.As3());
        if (value.Is4()) return TRole4.From(value.As4());
        return TRole5.From(value.As5());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value) || TRole3.Is(value) || TRole4.Is(value) || TRole5.Is(value);
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From1(TRole1.As(value));
        if (TRole2.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From2(TRole2.As(value));
        if (TRole3.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From3(TRole3.As(value));
        if (TRole4.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From4(TRole4.As(value));
        return Union<TArm1, TArm2, TArm3, TArm4, TArm5>.From5(TRole5.As(value));
    }
}

public readonly struct CompositeSourceUnionRole6<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TRole1, TRole2, TRole3, TRole4, TRole5, TRole6> : ISourceUnionRole<TStorage, Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
    where TRole6 : struct, ISourceUnionRole<TStorage, TArm6>
{
    public static TStorage From(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        if (value.Is2()) return TRole2.From(value.As2());
        if (value.Is3()) return TRole3.From(value.As3());
        if (value.Is4()) return TRole4.From(value.As4());
        if (value.Is5()) return TRole5.From(value.As5());
        return TRole6.From(value.As6());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value) || TRole3.Is(value) || TRole4.Is(value) || TRole5.Is(value) || TRole6.Is(value);
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From1(TRole1.As(value));
        if (TRole2.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From2(TRole2.As(value));
        if (TRole3.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From3(TRole3.As(value));
        if (TRole4.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From4(TRole4.As(value));
        if (TRole5.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From5(TRole5.As(value));
        return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>.From6(TRole6.As(value));
    }
}

public readonly struct CompositeSourceUnionRole7<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TRole1, TRole2, TRole3, TRole4, TRole5, TRole6, TRole7> : ISourceUnionRole<TStorage, Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
    where TRole6 : struct, ISourceUnionRole<TStorage, TArm6>
    where TRole7 : struct, ISourceUnionRole<TStorage, TArm7>
{
    public static TStorage From(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        if (value.Is2()) return TRole2.From(value.As2());
        if (value.Is3()) return TRole3.From(value.As3());
        if (value.Is4()) return TRole4.From(value.As4());
        if (value.Is5()) return TRole5.From(value.As5());
        if (value.Is6()) return TRole6.From(value.As6());
        return TRole7.From(value.As7());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value) || TRole3.Is(value) || TRole4.Is(value) || TRole5.Is(value) || TRole6.Is(value) || TRole7.Is(value);
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From1(TRole1.As(value));
        if (TRole2.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From2(TRole2.As(value));
        if (TRole3.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From3(TRole3.As(value));
        if (TRole4.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From4(TRole4.As(value));
        if (TRole5.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From5(TRole5.As(value));
        if (TRole6.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From6(TRole6.As(value));
        return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>.From7(TRole7.As(value));
    }
}

public readonly struct CompositeSourceUnionRole8<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8, TRole1, TRole2, TRole3, TRole4, TRole5, TRole6, TRole7, TRole8> : ISourceUnionRole<TStorage, Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
    where TRole6 : struct, ISourceUnionRole<TStorage, TArm6>
    where TRole7 : struct, ISourceUnionRole<TStorage, TArm7>
    where TRole8 : struct, ISourceUnionRole<TStorage, TArm8>
{
    public static TStorage From(Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> value)
    {
        if (value.Is1()) return TRole1.From(value.As1());
        if (value.Is2()) return TRole2.From(value.As2());
        if (value.Is3()) return TRole3.From(value.As3());
        if (value.Is4()) return TRole4.From(value.As4());
        if (value.Is5()) return TRole5.From(value.As5());
        if (value.Is6()) return TRole6.From(value.As6());
        if (value.Is7()) return TRole7.From(value.As7());
        return TRole8.From(value.As8());
    }
    public static bool Is(TStorage value) => TRole1.Is(value) || TRole2.Is(value) || TRole3.Is(value) || TRole4.Is(value) || TRole5.Is(value) || TRole6.Is(value) || TRole7.Is(value) || TRole8.Is(value);
    public static Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8> As(TStorage value)
    {
        if (TRole1.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From1(TRole1.As(value));
        if (TRole2.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From2(TRole2.As(value));
        if (TRole3.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From3(TRole3.As(value));
        if (TRole4.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From4(TRole4.As(value));
        if (TRole5.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From5(TRole5.As(value));
        if (TRole6.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From6(TRole6.As(value));
        if (TRole7.Is(value)) return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From7(TRole7.As(value));
        return Union<TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>.From8(TRole8.As(value));
    }
}
