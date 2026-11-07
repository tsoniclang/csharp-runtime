namespace Tsonic.CSharp.Runtime;

public interface ISourceUnion2<TStorage, TArm1, TArm2>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
}

public readonly struct SourceUnionOperations2<TStorage, TArm1, TArm2, TRole1, TRole2> : ISourceUnion2<TStorage, TArm1, TArm2>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
}

public interface ISourceUnion3<TStorage, TArm1, TArm2, TArm3>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
    static abstract TStorage From3(TArm3 value);
    static abstract bool Is3(TStorage value);
    static abstract TArm3 As3(TStorage value);
}

public readonly struct SourceUnionOperations3<TStorage, TArm1, TArm2, TArm3, TRole1, TRole2, TRole3> : ISourceUnion3<TStorage, TArm1, TArm2, TArm3>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
    public static TStorage From3(TArm3 value) => TRole3.From(value);
    public static bool Is3(TStorage value) => TRole3.Is(value);
    public static TArm3 As3(TStorage value) => TRole3.As(value);
}

public interface ISourceUnion4<TStorage, TArm1, TArm2, TArm3, TArm4>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
    static abstract TStorage From3(TArm3 value);
    static abstract bool Is3(TStorage value);
    static abstract TArm3 As3(TStorage value);
    static abstract TStorage From4(TArm4 value);
    static abstract bool Is4(TStorage value);
    static abstract TArm4 As4(TStorage value);
}

public readonly struct SourceUnionOperations4<TStorage, TArm1, TArm2, TArm3, TArm4, TRole1, TRole2, TRole3, TRole4> : ISourceUnion4<TStorage, TArm1, TArm2, TArm3, TArm4>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
    public static TStorage From3(TArm3 value) => TRole3.From(value);
    public static bool Is3(TStorage value) => TRole3.Is(value);
    public static TArm3 As3(TStorage value) => TRole3.As(value);
    public static TStorage From4(TArm4 value) => TRole4.From(value);
    public static bool Is4(TStorage value) => TRole4.Is(value);
    public static TArm4 As4(TStorage value) => TRole4.As(value);
}

public interface ISourceUnion5<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
    static abstract TStorage From3(TArm3 value);
    static abstract bool Is3(TStorage value);
    static abstract TArm3 As3(TStorage value);
    static abstract TStorage From4(TArm4 value);
    static abstract bool Is4(TStorage value);
    static abstract TArm4 As4(TStorage value);
    static abstract TStorage From5(TArm5 value);
    static abstract bool Is5(TStorage value);
    static abstract TArm5 As5(TStorage value);
}

public readonly struct SourceUnionOperations5<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TRole1, TRole2, TRole3, TRole4, TRole5> : ISourceUnion5<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
    public static TStorage From3(TArm3 value) => TRole3.From(value);
    public static bool Is3(TStorage value) => TRole3.Is(value);
    public static TArm3 As3(TStorage value) => TRole3.As(value);
    public static TStorage From4(TArm4 value) => TRole4.From(value);
    public static bool Is4(TStorage value) => TRole4.Is(value);
    public static TArm4 As4(TStorage value) => TRole4.As(value);
    public static TStorage From5(TArm5 value) => TRole5.From(value);
    public static bool Is5(TStorage value) => TRole5.Is(value);
    public static TArm5 As5(TStorage value) => TRole5.As(value);
}

public interface ISourceUnion6<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
    static abstract TStorage From3(TArm3 value);
    static abstract bool Is3(TStorage value);
    static abstract TArm3 As3(TStorage value);
    static abstract TStorage From4(TArm4 value);
    static abstract bool Is4(TStorage value);
    static abstract TArm4 As4(TStorage value);
    static abstract TStorage From5(TArm5 value);
    static abstract bool Is5(TStorage value);
    static abstract TArm5 As5(TStorage value);
    static abstract TStorage From6(TArm6 value);
    static abstract bool Is6(TStorage value);
    static abstract TArm6 As6(TStorage value);
}

public readonly struct SourceUnionOperations6<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TRole1, TRole2, TRole3, TRole4, TRole5, TRole6> : ISourceUnion6<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
    where TRole6 : struct, ISourceUnionRole<TStorage, TArm6>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
    public static TStorage From3(TArm3 value) => TRole3.From(value);
    public static bool Is3(TStorage value) => TRole3.Is(value);
    public static TArm3 As3(TStorage value) => TRole3.As(value);
    public static TStorage From4(TArm4 value) => TRole4.From(value);
    public static bool Is4(TStorage value) => TRole4.Is(value);
    public static TArm4 As4(TStorage value) => TRole4.As(value);
    public static TStorage From5(TArm5 value) => TRole5.From(value);
    public static bool Is5(TStorage value) => TRole5.Is(value);
    public static TArm5 As5(TStorage value) => TRole5.As(value);
    public static TStorage From6(TArm6 value) => TRole6.From(value);
    public static bool Is6(TStorage value) => TRole6.Is(value);
    public static TArm6 As6(TStorage value) => TRole6.As(value);
}

public interface ISourceUnion7<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
    static abstract TStorage From3(TArm3 value);
    static abstract bool Is3(TStorage value);
    static abstract TArm3 As3(TStorage value);
    static abstract TStorage From4(TArm4 value);
    static abstract bool Is4(TStorage value);
    static abstract TArm4 As4(TStorage value);
    static abstract TStorage From5(TArm5 value);
    static abstract bool Is5(TStorage value);
    static abstract TArm5 As5(TStorage value);
    static abstract TStorage From6(TArm6 value);
    static abstract bool Is6(TStorage value);
    static abstract TArm6 As6(TStorage value);
    static abstract TStorage From7(TArm7 value);
    static abstract bool Is7(TStorage value);
    static abstract TArm7 As7(TStorage value);
}

public readonly struct SourceUnionOperations7<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TRole1, TRole2, TRole3, TRole4, TRole5, TRole6, TRole7> : ISourceUnion7<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
    where TRole6 : struct, ISourceUnionRole<TStorage, TArm6>
    where TRole7 : struct, ISourceUnionRole<TStorage, TArm7>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
    public static TStorage From3(TArm3 value) => TRole3.From(value);
    public static bool Is3(TStorage value) => TRole3.Is(value);
    public static TArm3 As3(TStorage value) => TRole3.As(value);
    public static TStorage From4(TArm4 value) => TRole4.From(value);
    public static bool Is4(TStorage value) => TRole4.Is(value);
    public static TArm4 As4(TStorage value) => TRole4.As(value);
    public static TStorage From5(TArm5 value) => TRole5.From(value);
    public static bool Is5(TStorage value) => TRole5.Is(value);
    public static TArm5 As5(TStorage value) => TRole5.As(value);
    public static TStorage From6(TArm6 value) => TRole6.From(value);
    public static bool Is6(TStorage value) => TRole6.Is(value);
    public static TArm6 As6(TStorage value) => TRole6.As(value);
    public static TStorage From7(TArm7 value) => TRole7.From(value);
    public static bool Is7(TStorage value) => TRole7.Is(value);
    public static TArm7 As7(TStorage value) => TRole7.As(value);
}

public interface ISourceUnion8<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>
{
    static abstract TStorage From1(TArm1 value);
    static abstract bool Is1(TStorage value);
    static abstract TArm1 As1(TStorage value);
    static abstract TStorage From2(TArm2 value);
    static abstract bool Is2(TStorage value);
    static abstract TArm2 As2(TStorage value);
    static abstract TStorage From3(TArm3 value);
    static abstract bool Is3(TStorage value);
    static abstract TArm3 As3(TStorage value);
    static abstract TStorage From4(TArm4 value);
    static abstract bool Is4(TStorage value);
    static abstract TArm4 As4(TStorage value);
    static abstract TStorage From5(TArm5 value);
    static abstract bool Is5(TStorage value);
    static abstract TArm5 As5(TStorage value);
    static abstract TStorage From6(TArm6 value);
    static abstract bool Is6(TStorage value);
    static abstract TArm6 As6(TStorage value);
    static abstract TStorage From7(TArm7 value);
    static abstract bool Is7(TStorage value);
    static abstract TArm7 As7(TStorage value);
    static abstract TStorage From8(TArm8 value);
    static abstract bool Is8(TStorage value);
    static abstract TArm8 As8(TStorage value);
}

public readonly struct SourceUnionOperations8<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8, TRole1, TRole2, TRole3, TRole4, TRole5, TRole6, TRole7, TRole8> : ISourceUnion8<TStorage, TArm1, TArm2, TArm3, TArm4, TArm5, TArm6, TArm7, TArm8>
    where TRole1 : struct, ISourceUnionRole<TStorage, TArm1>
    where TRole2 : struct, ISourceUnionRole<TStorage, TArm2>
    where TRole3 : struct, ISourceUnionRole<TStorage, TArm3>
    where TRole4 : struct, ISourceUnionRole<TStorage, TArm4>
    where TRole5 : struct, ISourceUnionRole<TStorage, TArm5>
    where TRole6 : struct, ISourceUnionRole<TStorage, TArm6>
    where TRole7 : struct, ISourceUnionRole<TStorage, TArm7>
    where TRole8 : struct, ISourceUnionRole<TStorage, TArm8>
{
    public static TStorage From1(TArm1 value) => TRole1.From(value);
    public static bool Is1(TStorage value) => TRole1.Is(value);
    public static TArm1 As1(TStorage value) => TRole1.As(value);
    public static TStorage From2(TArm2 value) => TRole2.From(value);
    public static bool Is2(TStorage value) => TRole2.Is(value);
    public static TArm2 As2(TStorage value) => TRole2.As(value);
    public static TStorage From3(TArm3 value) => TRole3.From(value);
    public static bool Is3(TStorage value) => TRole3.Is(value);
    public static TArm3 As3(TStorage value) => TRole3.As(value);
    public static TStorage From4(TArm4 value) => TRole4.From(value);
    public static bool Is4(TStorage value) => TRole4.Is(value);
    public static TArm4 As4(TStorage value) => TRole4.As(value);
    public static TStorage From5(TArm5 value) => TRole5.From(value);
    public static bool Is5(TStorage value) => TRole5.Is(value);
    public static TArm5 As5(TStorage value) => TRole5.As(value);
    public static TStorage From6(TArm6 value) => TRole6.From(value);
    public static bool Is6(TStorage value) => TRole6.Is(value);
    public static TArm6 As6(TStorage value) => TRole6.As(value);
    public static TStorage From7(TArm7 value) => TRole7.From(value);
    public static bool Is7(TStorage value) => TRole7.Is(value);
    public static TArm7 As7(TStorage value) => TRole7.As(value);
    public static TStorage From8(TArm8 value) => TRole8.From(value);
    public static bool Is8(TStorage value) => TRole8.Is(value);
    public static TArm8 As8(TStorage value) => TRole8.As(value);
}
