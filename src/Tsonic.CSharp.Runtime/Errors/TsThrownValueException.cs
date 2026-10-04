using System;

namespace Tsonic.CSharp.Runtime
{
    public sealed class TsThrownValueException : Exception
    {
        public TsValue value { get; }

        public TsThrownValueException(TsValue value)
            : base("JavaScript value was thrown.")
        {
            this.value = value;
        }

        public static Exception from(TsValue value)
        {
            return TsValue.UnwrapClosedValue(value) is Exception exception
                ? exception
                : new TsThrownValueException(value);
        }

        public static TsValue toValue(Exception exception)
        {
            return exception switch
            {
                TsThrownValueException thrown => thrown.value,
                Error error => TsValue.from(error),
                _ => TsValue.from(exception)
            };
        }
    }
}
