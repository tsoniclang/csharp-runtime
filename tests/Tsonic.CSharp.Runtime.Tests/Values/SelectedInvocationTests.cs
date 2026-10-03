using System;
using Tsonic.CSharp.Runtime;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests
{
    public class SelectedInvocationTests
    {
        [Fact]
        public void SelectedInvocation_PreservesOriginalReceiverAndCapturedCallee()
        {
            var receiver = TsValue.CreateDynamicObject("value", ulong.MaxValue);
            var original = new TsFunction((selected, arguments) =>
            {
                Assert.Same(receiver.unwrap(), selected.unwrap());
                Assert.Equal(ulong.MaxValue, selected.ReadDynamicSlot("value").unwrap());
                return arguments[0];
            });
            receiver.WriteDynamicSlot("call", original);
            var callee = receiver.ReadDynamicSlot("call");
            receiver.WriteDynamicSlot("call", new TsFunction(_ => throw new InvalidOperationException("replacement")));
            Assert.Equal(ulong.MaxValue, callee.InvokeDynamicWithThis(receiver, ulong.MaxValue).unwrap());
            Assert.Same(original, callee.unwrap());
        }

        [Fact]
        public void SelectedInvocation_PreservesFailureAndRejectsNonCallablePayload()
        {
            var failure = new InvalidOperationException("native failure");
            var callee = TsValue.from(new TsFunction(_ => throw failure));
            Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => callee.InvokeDynamicWithThis(TsValue.undefined())));
            Assert.Throws<TypeError>(() => TsValue.from(3).InvokeDynamicWithThis(TsValue.undefined()));
            Assert.Throws<TypeError>(() => TsValue.undefined().InvokeDynamicWithThis(TsValue.undefined()));
        }
    }
}
