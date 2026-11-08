using System;

namespace Tsonic.CSharp.Runtime
{
    public sealed class TsThrownValueException : Exception
    {
        public object? value { get; }
        private readonly TsValue? _sourceValue;

        public TsThrownValueException(object? value)
            : base("JavaScript value was thrown.")
        {
            this.value = TsValue.UnwrapClosedValue(value);
            _sourceValue = value is TsValue closed ? closed : null;
        }

        public static Exception from<TValue>(TValue value)
        {
            var payload = value is TsValue closed
                ? TsValue.UnwrapClosedValue(closed)
                : TsValue.UnwrapClosedValue(value);
            return payload is Exception exception
                ? exception
                : value is TsValue source ? new TsThrownValueException(source) : new TsThrownValueException(payload);
        }

        public static TsValue toValue(Exception exception)
        {
            return exception switch
            {
                TsThrownValueException thrown => thrown._sourceValue ?? TsValue.fromnative(thrown.value),
                Error error => TsValue.fromnative(error),
                _ => TsValue.fromnative(exception)
            };
        }
    }
}
