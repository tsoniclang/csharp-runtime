using System;
using System.Runtime.CompilerServices;

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
            if (typeof(TValue) == typeof(TsValue))
                return FromSourceValue(Unsafe.As<TValue, TsValue>(ref value));
            if (value is TsValue closed) return FromSourceValue(closed);
            var payload = TsValue.UnwrapClosedValue(value);
            return payload is Exception exception
                ? exception
                : new TsThrownValueException(payload);
        }

        private TsThrownValueException(TsValue value)
            : base("JavaScript value was thrown.")
        {
            this.value = TsValue.UnwrapClosedValue(value);
            _sourceValue = value;
        }

        private static Exception FromSourceValue(TsValue value)
        {
            var payload = TsValue.UnwrapClosedValue(value);
            return payload is Exception exception ? exception : new TsThrownValueException(value);
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
