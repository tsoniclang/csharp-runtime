using System;
using System.Collections.Generic;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public class SourceUnionProjectionTests
{
    [Fact]
    public void ClosedRolesRetainEveryNativeOrderedArm()
    {
        CheckRole<Union<int, int>, int, SourceUnionArm1Of2<int, int>>(Union<int, int>.From1(17), 17);
        CheckRole<Union<int, int>, int, SourceUnionArm2Of2<int, int>>(Union<int, int>.From2(17), 17);
        CheckRole<Union<int, int, int>, int, SourceUnionArm1Of3<int, int, int>>(Union<int, int, int>.From1(17), 17);
        CheckRole<Union<int, int, int>, int, SourceUnionArm2Of3<int, int, int>>(Union<int, int, int>.From2(17), 17);
        CheckRole<Union<int, int, int>, int, SourceUnionArm3Of3<int, int, int>>(Union<int, int, int>.From3(17), 17);
        CheckRole<Union<int, int, int, int>, int, SourceUnionArm1Of4<int, int, int, int>>(Union<int, int, int, int>.From1(17), 17);
        CheckRole<Union<int, int, int, int>, int, SourceUnionArm2Of4<int, int, int, int>>(Union<int, int, int, int>.From2(17), 17);
        CheckRole<Union<int, int, int, int>, int, SourceUnionArm3Of4<int, int, int, int>>(Union<int, int, int, int>.From3(17), 17);
        CheckRole<Union<int, int, int, int>, int, SourceUnionArm4Of4<int, int, int, int>>(Union<int, int, int, int>.From4(17), 17);
        CheckRole<Union<int, int, int, int, int>, int, SourceUnionArm1Of5<int, int, int, int, int>>(Union<int, int, int, int, int>.From1(17), 17);
        CheckRole<Union<int, int, int, int, int>, int, SourceUnionArm2Of5<int, int, int, int, int>>(Union<int, int, int, int, int>.From2(17), 17);
        CheckRole<Union<int, int, int, int, int>, int, SourceUnionArm3Of5<int, int, int, int, int>>(Union<int, int, int, int, int>.From3(17), 17);
        CheckRole<Union<int, int, int, int, int>, int, SourceUnionArm4Of5<int, int, int, int, int>>(Union<int, int, int, int, int>.From4(17), 17);
        CheckRole<Union<int, int, int, int, int>, int, SourceUnionArm5Of5<int, int, int, int, int>>(Union<int, int, int, int, int>.From5(17), 17);
        CheckRole<Union<int, int, int, int, int, int>, int, SourceUnionArm1Of6<int, int, int, int, int, int>>(Union<int, int, int, int, int, int>.From1(17), 17);
        CheckRole<Union<int, int, int, int, int, int>, int, SourceUnionArm2Of6<int, int, int, int, int, int>>(Union<int, int, int, int, int, int>.From2(17), 17);
        CheckRole<Union<int, int, int, int, int, int>, int, SourceUnionArm3Of6<int, int, int, int, int, int>>(Union<int, int, int, int, int, int>.From3(17), 17);
        CheckRole<Union<int, int, int, int, int, int>, int, SourceUnionArm4Of6<int, int, int, int, int, int>>(Union<int, int, int, int, int, int>.From4(17), 17);
        CheckRole<Union<int, int, int, int, int, int>, int, SourceUnionArm5Of6<int, int, int, int, int, int>>(Union<int, int, int, int, int, int>.From5(17), 17);
        CheckRole<Union<int, int, int, int, int, int>, int, SourceUnionArm6Of6<int, int, int, int, int, int>>(Union<int, int, int, int, int, int>.From6(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm1Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From1(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm2Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From2(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm3Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From3(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm4Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From4(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm5Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From5(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm6Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From6(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int>, int, SourceUnionArm7Of7<int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int>.From7(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm1Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From1(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm2Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From2(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm3Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From3(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm4Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From4(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm5Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From5(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm6Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From6(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm7Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From7(17), 17);
        CheckRole<Union<int, int, int, int, int, int, int, int>, int, SourceUnionArm8Of8<int, int, int, int, int, int, int, int>>(Union<int, int, int, int, int, int, int, int>.From8(17), 17);
    }

    [Fact]
    public void CoincidentRolesUseTheSameScalarStorage()
    {
        var text = new string('a', 3);
        CheckRole<string, string, IdentitySourceUnionRole<string>>(text, text);
        Assert.Same(text, SourceUnionOperations2<string, string, string, IdentitySourceUnionRole<string>, IdentitySourceUnionRole<string>>.From1(text));
        Assert.Same(text, SourceUnionOperations2<string, string, string, IdentitySourceUnionRole<string>, IdentitySourceUnionRole<string>>.From2(text));
        Assert.True(SourceUnionOperations2<string, string, string, IdentitySourceUnionRole<string>, IdentitySourceUnionRole<string>>.Is1(text));
        Assert.True(SourceUnionOperations2<string, string, string, IdentitySourceUnionRole<string>, IdentitySourceUnionRole<string>>.Is2(text));
    }

    [Fact]
    public void NestedRolesComposeWithoutChangingStorageTags()
    {
        var role = Union<int, double>.From1(23);
        var stored = CompositeSourceUnionRole2<Union<string, int, double>, int, double,
            SourceUnionArm2Of3<string, int, double>, SourceUnionArm3Of3<string, int, double>>.From(role);
        Assert.True(stored.Is2());
        Assert.False(stored.Is1());
        var restored = CompositeSourceUnionRole2<Union<string, int, double>, int, double,
            SourceUnionArm2Of3<string, int, double>, SourceUnionArm3Of3<string, int, double>>.As(stored);
        Assert.True(restored.Is1());
        Assert.Equal(23, restored.As1());
        Assert.False(CompositeSourceUnionRole2<Union<string, int, double>, int, double,
            SourceUnionArm2Of3<string, int, double>, SourceUnionArm3Of3<string, int, double>>.Is(Union<string, int, double>.From1("text")));
    }

    [Fact]
    public void OptionalRolesKeepOneNativeAbsenceAndPresentZero()
    {
        var absent = OptionalSourceUnionRole<Union<string, int>?, Union<string, int>, int?, int,
            ValueOptionalStorage<Union<string, int>>, ValueOptionalStorage<int>, SourceUnionArm2Of2<string, int>>.From(null);
        Assert.False(absent.HasValue);
        var zero = OptionalSourceUnionRole<Union<string, int>?, Union<string, int>, int?, int,
            ValueOptionalStorage<Union<string, int>>, ValueOptionalStorage<int>, SourceUnionArm2Of2<string, int>>.From(0);
        Assert.True(zero.HasValue);
        var present = zero ?? throw new InvalidOperationException("The native zero payload must be present.");
        Assert.True(present.Is2());
        Assert.Equal(0, present.As2());
        Assert.False(OptionalStorageSourceUnionRole<Union<string, int>?, Union<string, int>, string,
            ValueOptionalStorage<Union<string, int>>, SourceUnionArm1Of2<string, int>>.Is(absent));
        Assert.True(AbsentSourceUnionRole<Union<string, int>?, Union<string, int>, ValueOptionalStorage<Union<string, int>>>.Is(absent));
        Assert.Throws<InvalidOperationException>(() => SourceUnionArm2Of2<string, int>.As(Union<string, int>.From1("text")));
    }

    [Fact]
    public void StaticRoleRoundTripsHaveNoBoxingOrAllocation()
    {
        var total = 0;
        for (var index = 0; index < 4096; index++) total += RoundTrip<Union<string, int>, int, SourceUnionArm2Of2<string, int>>(17);
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var index = 0; index < 65536; index++) total += RoundTrip<Union<string, int>, int, SourceUnionArm2Of2<string, int>>(17);
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(17 * (4096 + 65536), total);
        Assert.Equal(0, allocated);
    }

    private static void CheckRole<TStorage, TRole, TOperations>(TStorage value, TRole expected)
        where TOperations : struct, ISourceUnionRole<TStorage, TRole>
    {
        Assert.True(TOperations.Is(value));
        Assert.True(EqualityComparer<TRole>.Default.Equals(expected, TOperations.As(value)));
        Assert.True(TOperations.Is(TOperations.From(expected)));
    }

    private static TRole RoundTrip<TStorage, TRole, TOperations>(TRole value)
        where TOperations : struct, ISourceUnionRole<TStorage, TRole>
    {
        var stored = TOperations.From(value);
        if (!TOperations.Is(stored)) throw new InvalidOperationException("The selected native role did not round-trip.");
        return TOperations.As(stored);
    }
}
