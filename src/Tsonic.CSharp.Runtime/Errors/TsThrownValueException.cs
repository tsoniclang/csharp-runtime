using System;

namespace Tsonic.CSharp.Runtime
{
    public sealed class TsThrownValueException : Exception
    {
        public object? value { get; }

        public TsThrownValueException(object? value)
            : base("JavaScript value was thrown.")
        {
            this.value = TsValue.UnwrapClosedValue(value);
        }

        public static Exception from<TValue>(TValue value)
        {
            var payload = value is TsValue closed
                ? TsValue.UnwrapClosedValue(closed)
                : TsValue.UnwrapClosedValue(value);
            return payload is Exception exception
                ? exception
                : new TsThrownValueException(payload);
        }

        public static TsValue toValue(Exception exception)
        {
            return exception switch
            {
                TsThrownValueException thrown => TsValue.from(thrown.value),
                Error error => TsValue.from(error),
                _ => TsValue.from(exception)
            };
        }
    }
}
