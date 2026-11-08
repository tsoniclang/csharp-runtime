using System;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests;

public sealed class SourceUnionRoleTests
{
    [Fact]
    public void EveryImmediateRolePreservesItsExactPhysicalSlotAndCategory()
    {
        CheckRole<Union<long, long>, long, SourceUnionRole1<long, long>>(7L, Union<long, long>.From2(7L));
        CheckCategory<Union<long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long>, long, SourceUnionRole2<long, long>>(7L, Union<long, long>.From1(7L));
        CheckCategory<Union<long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long>.From2(7L), SourceValueCategory.BigInt);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>.Category(default));
        CheckRole<Union<long, long, long>, long, SourceUnionRole1<long, long, long>>(7L, Union<long, long, long>.From2(7L));
        CheckCategory<Union<long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long>, long, SourceUnionRole2<long, long, long>>(7L, Union<long, long, long>.From3(7L));
        CheckCategory<Union<long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long>.From2(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long>, long, SourceUnionRole3<long, long, long>>(7L, Union<long, long, long>.From1(7L));
        CheckCategory<Union<long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long>.From3(7L), SourceValueCategory.Number);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>.Category(default));
        CheckRole<Union<long, long, long, long>, long, SourceUnionRole1<long, long, long, long>>(7L, Union<long, long, long, long>.From2(7L));
        CheckCategory<Union<long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long>, long, SourceUnionRole2<long, long, long, long>>(7L, Union<long, long, long, long>.From3(7L));
        CheckCategory<Union<long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long>.From2(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long>, long, SourceUnionRole3<long, long, long, long>>(7L, Union<long, long, long, long>.From4(7L));
        CheckCategory<Union<long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long>.From3(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long>, long, SourceUnionRole4<long, long, long, long>>(7L, Union<long, long, long, long>.From1(7L));
        CheckCategory<Union<long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long>.From4(7L), SourceValueCategory.BigInt);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>.Category(default));
        CheckRole<Union<long, long, long, long, long>, long, SourceUnionRole1<long, long, long, long, long>>(7L, Union<long, long, long, long, long>.From2(7L));
        CheckCategory<Union<long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long>, long, SourceUnionRole2<long, long, long, long, long>>(7L, Union<long, long, long, long, long>.From3(7L));
        CheckCategory<Union<long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long>.From2(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long>, long, SourceUnionRole3<long, long, long, long, long>>(7L, Union<long, long, long, long, long>.From4(7L));
        CheckCategory<Union<long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long>.From3(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long>, long, SourceUnionRole4<long, long, long, long, long>>(7L, Union<long, long, long, long, long>.From5(7L));
        CheckCategory<Union<long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long>.From4(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long>, long, SourceUnionRole5<long, long, long, long, long>>(7L, Union<long, long, long, long, long>.From1(7L));
        CheckCategory<Union<long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long>.From5(7L), SourceValueCategory.Number);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>.Category(default));
        CheckRole<Union<long, long, long, long, long, long>, long, SourceUnionRole1<long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long>.From2(7L));
        CheckCategory<Union<long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long>, long, SourceUnionRole2<long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long>.From3(7L));
        CheckCategory<Union<long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long>.From2(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long>, long, SourceUnionRole3<long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long>.From4(7L));
        CheckCategory<Union<long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long>.From3(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long>, long, SourceUnionRole4<long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long>.From5(7L));
        CheckCategory<Union<long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long>.From4(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long>, long, SourceUnionRole5<long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long>.From6(7L));
        CheckCategory<Union<long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long>.From5(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long>, long, SourceUnionRole6<long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long>.From1(7L));
        CheckCategory<Union<long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long>.From6(7L), SourceValueCategory.BigInt);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>.Category(default));
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole1<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From2(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole2<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From3(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From2(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole3<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From4(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From3(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole4<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From5(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From4(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole5<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From6(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From5(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole6<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From7(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From6(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long, long>, long, SourceUnionRole7<long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long>.From1(7L));
        CheckCategory<Union<long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>>(Union<long, long, long, long, long, long, long>.From7(7L), SourceValueCategory.Number);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>>.Category(default));
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole1<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From2(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From1(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole2<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From3(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From2(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole3<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From4(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From3(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole4<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From5(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From4(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole5<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From6(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From5(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole6<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From7(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From6(7L), SourceValueCategory.BigInt);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole7<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From8(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From7(7L), SourceValueCategory.Number);
        CheckRole<Union<long, long, long, long, long, long, long, long>, long, SourceUnionRole8<long, long, long, long, long, long, long, long>>(7L, Union<long, long, long, long, long, long, long, long>.From1(7L));
        CheckCategory<Union<long, long, long, long, long, long, long, long>, SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>>(Union<long, long, long, long, long, long, long, long>.From8(7L), SourceValueCategory.BigInt);
        Assert.Throws<InvalidOperationException>(() => SourceUnionValueCategory<long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>, long, SourceNumberCategory<long>, long, SourceBigIntCategory<long>>.Category(default));
    }

    [Fact]
    public void NullableRolesComposeExistingStorageWithoutInventingAnAbsenceTag()
    {
        var value = SourceUnionOptionalRole<Union<int, string>, Union<int, string>?, int, ValueOptionalStorage<Union<int, string>>, SourceUnionRole1<int, string>>.From(0);
        Assert.True(value.HasValue);
        Assert.True(SourceUnionOptionalRole<Union<int, string>, Union<int, string>?, int, ValueOptionalStorage<Union<int, string>>, SourceUnionRole1<int, string>>.Is(value));
        Assert.Equal(0, SourceUnionOptionalRole<Union<int, string>, Union<int, string>?, int, ValueOptionalStorage<Union<int, string>>, SourceUnionRole1<int, string>>.As(value));
        Assert.False(SourceUnionOptionalRole<Union<int, string>, Union<int, string>?, int, ValueOptionalStorage<Union<int, string>>, SourceUnionRole1<int, string>>.Is(null));
        Assert.Throws<InvalidOperationException>(() => SourceUnionOptionalRole<Union<int, string>, Union<int, string>?, int, ValueOptionalStorage<Union<int, string>>, SourceUnionRole1<int, string>>.As(null));
        Assert.Null(SourceUnionOptionalAbsence<Union<int, string>, Union<int, string>?, ValueOptionalStorage<Union<int, string>>>.From(null));
        Assert.True(SourceUnionOptionalAbsence<Union<int, string>, Union<int, string>?, ValueOptionalStorage<Union<int, string>>>.Is(null));
        Assert.Null(SourceUnionOptionalAbsence<Union<int, string>, Union<int, string>?, ValueOptionalStorage<Union<int, string>>>.As(null));
        Assert.False(SourceUnionOptionalAbsence<Union<int, string>, Union<int, string>?, ValueOptionalStorage<Union<int, string>>>.Is(value));
        Assert.Throws<ArgumentException>(() => SourceUnionOptionalAbsence<Union<int, string>, Union<int, string>?, ValueOptionalStorage<Union<int, string>>>.From(0));
        Assert.Throws<InvalidOperationException>(() => SourceUnionOptionalAbsence<Union<int, string>, Union<int, string>?, ValueOptionalStorage<Union<int, string>>>.As(value));
        Assert.Equal(0, RoundTripOptional<int, int?, ValueOptionalStorage<int>>(0));
        Assert.Equal("", RoundTripOptional<string, string?, ReferenceOptionalStorage<string>>(""));
        Assert.False(RoundTripOptional<bool, bool?, ValueOptionalStorage<bool>>(false));
        var original = new TsObject();
        var retained = SourceUnionRole2<int, TsObject>.As(SourceUnionRole2<int, TsObject>.From(original));
        Assert.Same(original, retained);
        Assert.Same(original, SourceUnionIdentity<TsObject>.As(SourceUnionIdentity<TsObject>.From(original)));
    }

    private static TPayload RoundTripOptional<TPayload, TStorage, TOps>(TPayload value)
        where TOps : struct, IOptionalStorage<TPayload, TStorage>
    {
        var present = SourceUnionOptionalRole<TPayload, TStorage, TPayload, TOps, SourceUnionIdentity<TPayload>>.From(value);
        Assert.True(SourceUnionOptionalRole<TPayload, TStorage, TPayload, TOps, SourceUnionIdentity<TPayload>>.Is(present));
        var absent = SourceUnionOptionalAbsence<TPayload, TStorage, TOps>.From(null);
        Assert.True(SourceUnionOptionalAbsence<TPayload, TStorage, TOps>.Is(absent));
        Assert.False(SourceUnionOptionalRole<TPayload, TStorage, TPayload, TOps, SourceUnionIdentity<TPayload>>.Is(absent));
        return SourceUnionOptionalRole<TPayload, TStorage, TPayload, TOps, SourceUnionIdentity<TPayload>>.As(present);
    }

    private static void CheckRole<TStorage, TValue, TWitness>(TValue value, TStorage other)
        where TWitness : struct, ISourceUnionRole<TStorage, TValue>
    {
        var stored = TWitness.From(value);
        Assert.True(TWitness.Is(stored));
        Assert.Equal(value, TWitness.As(stored));
        Assert.False(TWitness.Is(other));
        Assert.Throws<InvalidOperationException>(() => TWitness.As(other));
    }

    private static void CheckCategory<TStorage, TCategory>(TStorage value, SourceValueCategory category)
        where TCategory : struct, ISourceValueCategory<TStorage>
    {
        Assert.Equal(category, TCategory.Category(value));
        Assert.Equal(category, TsValue.fromsource<TStorage, TCategory>(value).Category);
    }
}
