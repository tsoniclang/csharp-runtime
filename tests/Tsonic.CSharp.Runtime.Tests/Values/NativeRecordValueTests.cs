using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests
{
    public class NativeRecordValueTests
    {
        [Fact]
        public void Admission_RetainsTheOriginalDictionaryAndExactPayload()
        {
            var record = new Dictionary<string, TsValue>
            {
                ["present"] = TsValue.fromnative(ulong.MaxValue),
                ["absent"] = TsValue.undefined(),
            };
            var value = TsValue.fromnative(record);
            var alias = TsValue.fromnative(record);
            Assert.Same(record, value.unwrap());
            Assert.Same(record, TsValue.CastDynamic<Dictionary<string, TsValue>>(value));
            Assert.True(TsValue.StrictEqual(value, alias));
            Assert.False(TsValue.StrictEqual(value, TsValue.fromnative(new Dictionary<string, TsValue>())));
            Assert.Equal("object", TsValue.ApplyDynamicTypeof(value));
            Assert.Equal(ulong.MaxValue, Assert.IsType<ulong>(value.ReadDynamicSlot("present").unwrap()));
            Assert.True(value.ReadDynamicSlot("absent").isUndefined());
            Assert.True(value.ReadDynamicSlot("missing").isUndefined());
            record["present"] = TsValue.fromnative(ulong.MaxValue - 1);
            Assert.Equal(ulong.MaxValue - 1, value.ReadDynamicSlotAs<ulong>("present"));
        }

        [Fact]
        public void WritesAndElementReads_UseTheSameBackingAndOneAbsence()
        {
            var record = new Dictionary<string, TsValue>();
            var value = TsValue.fromnative(record);
            var original = TsValue.CreateDynamicObject("identity", "retained");
            Assert.Same(original.unwrap(), value.WriteDynamicSlot("child", original).unwrap());
            Assert.Same(original.unwrap(), record["child"].unwrap());
            value.WriteDynamicElement("wide", ulong.MaxValue);
            Assert.Equal(ulong.MaxValue, record["wide"].unwrap());
            Assert.Equal(ulong.MaxValue, value.ReadDynamicElement("wide").unwrap());
            value.WriteDynamicSlot("null", null);
            value.WriteDynamicSlot("undefined", TsValue.undefined());
            Assert.True(record["null"].isUndefined());
            Assert.True(record["undefined"].isUndefined());
            Assert.Same(TsValue.undefined().unwrap(), value.ReadDynamicSlot("null").unwrap());
            Assert.Throws<NotSupportedException>(() => value.WriteDynamicSlot("open", new object()));
            Assert.False(record.ContainsKey("open"));
        }

        [Fact]
        public void ReadOnlyAdmission_UsesTheOriginalLiveViewAndRejectsWrites()
        {
            var record = new Dictionary<string, TsValue> { ["live"] = TsValue.fromnative("before") };
            var view = new ReadOnlyDictionary<string, TsValue>(record);
            var value = TsValue.fromnative(view);
            Assert.Same(view, value.unwrap());
            record["live"] = TsValue.fromnative("after");
            Assert.Equal("after", value.ReadDynamicSlot("live").unwrap());
            Assert.Throws<NotSupportedException>(() => value.WriteDynamicSlot("live", "blocked"));
            Assert.Equal("after", record["live"].unwrap());
        }

        [Fact]
        public void NestedClosedUnion_RetainsRecordAliasesAndOptionalReads()
        {
            var record = new Dictionary<string, TsValue> { ["wide"] = TsValue.fromnative(ulong.MaxValue) };
            var union = Union<int, Dictionary<string, TsValue>>.From2(record);
            var value = TsValue.fromnative(union);
            Assert.Same(record, TsValue.UnwrapClosedValue(value));
            Assert.Equal(ulong.MaxValue, value.ReadDynamicSlotOptional("wide").unwrap());
            value.WriteDynamicSlot("wide", 1UL);
            Assert.Equal(1UL, record["wide"].unwrap());
            Assert.True(TsValue.undefined().ReadDynamicSlotOptional("wide").isUndefined());
        }

        [Fact]
        public void NativeRecordAdmissionAndReads_AddNoWrapperOrPerReadAllocation()
        {
            var record = new Dictionary<string, TsValue> { ["wide"] = TsValue.fromnative(ulong.MaxValue) };
            for (var index = 0; index < 1000; index++)
            {
                var warm = TsValue.fromnative(record);
                warm.ReadDynamicSlot("wide");
            }
            var before = GC.GetAllocatedBytesForCurrentThread();
            var total = 0;
            for (var index = 0; index < 10000; index++)
            {
                var value = TsValue.fromnative(record);
                if (ReferenceEquals(value.unwrap(), record) && value.ReadDynamicSlotAs<ulong>("wide") == ulong.MaxValue)
                    total++;
            }
            var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
            Assert.Equal(10000, total);
            Assert.Equal(0, allocated);
        }

        [Fact]
        public void UnrelatedDictionaryPayloads_StayOutsideTheClosedCarrierContract()
        {
            Assert.Throws<NotSupportedException>(() => TsValue.fromnative(new Dictionary<string, int>()));
            Assert.Throws<NotSupportedException>(() => TsValue.fromnative(new Dictionary<int, TsValue>()));
            Assert.Throws<NotSupportedException>(() => TsValue.fromnative(new object()));
        }
    }
}
