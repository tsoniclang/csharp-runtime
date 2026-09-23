using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tsonic.CSharp.Runtime
{
    /// <summary>
    /// Closed value carrier for values whose static TypeScript source type is broad.
    /// </summary>
    public readonly struct TsValue
    {
        private readonly object? _value;

        private TsValue(object? value)
        {
            _value = value;
        }

        public static TsValue from(object? value)
        {
            if (value is TsValue typed)
            {
                return typed;
            }

            if (!isSupported(value))
            {
                throw new NotSupportedException("TsValue requires a closed TypeScript runtime carrier.");
            }
            return new TsValue(value);
        }

        public static TsValue from<T1, T2>(Union<T1, T2> value)
        {
            return from(TsUnion.From<T1, T2>(value));
        }

        public static TsValue from<T1, T2>(Union<T1, T2>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue from<T1, T2, T3>(Union<T1, T2, T3> value)
        {
            return from(TsUnion.From<T1, T2, T3>(value));
        }

        public static TsValue from<T1, T2, T3>(Union<T1, T2, T3>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue from<T1, T2, T3, T4>(Union<T1, T2, T3, T4> value)
        {
            return from(TsUnion.From<T1, T2, T3, T4>(value));
        }

        public static TsValue from<T1, T2, T3, T4>(Union<T1, T2, T3, T4>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5>(Union<T1, T2, T3, T4, T5> value)
        {
            return from(TsUnion.From<T1, T2, T3, T4, T5>(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5>(Union<T1, T2, T3, T4, T5>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6>(Union<T1, T2, T3, T4, T5, T6> value)
        {
            return from(TsUnion.From<T1, T2, T3, T4, T5, T6>(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6>(Union<T1, T2, T3, T4, T5, T6>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7>(Union<T1, T2, T3, T4, T5, T6, T7> value)
        {
            return from(TsUnion.From<T1, T2, T3, T4, T5, T6, T7>(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7>(Union<T1, T2, T3, T4, T5, T6, T7>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7, T8>(Union<T1, T2, T3, T4, T5, T6, T7, T8> value)
        {
            return from(TsUnion.From<T1, T2, T3, T4, T5, T6, T7, T8>(value));
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7, T8>(Union<T1, T2, T3, T4, T5, T6, T7, T8>? value)
        {
            return value is null ? from(null) : from(TsUnion.From(value));
        }

        public static TsValue undefined()
        {
            return new TsValue(Undefined.value);
        }

        public static TsValue CreateDynamicObject(params object?[] keyValues)
        {
            if (keyValues.Length % 2 != 0)
            {
                throw new ArgumentException("Dynamic object construction requires exact key/value pairs.", nameof(keyValues));
            }

            var target = new TsObject();
            for (var index = 0; index < keyValues.Length; index += 2)
            {
                if (keyValues[index] is not string key)
                {
                    throw new ArgumentException("Dynamic object construction requires string property keys.", nameof(keyValues));
                }
                target.WriteDynamicSlot(key, keyValues[index + 1]);
            }
            return from(target);
        }

        public object? unwrap()
        {
            return _value;
        }

        public bool isUndefined()
        {
            return unwrapForOperation(_value) is Undefined;
        }

        public TsValue ReadDynamicSlot(string key)
        {
            return unwrapForOperation(_value) switch
            {
                Undefined => throw nullishReadError(key),
                null => throw nullishReadError(key),
                TsObject target => target.ReadDynamicSlot(key),
                TsArray target => target.ReadDynamicSlot(key),
                TsFunction target => target.ReadDynamicSlot(key),
                Error target => readErrorSlot(target, key),
                Exception target => readExceptionSlot(target, key),
                IDynamicObject target => target.TryReadDynamicSlot(key, out var value) ? from(value) : undefined(),
                IDictionary<string, object?> target => target.TryGetValue(key, out var value) ? from(value) : undefined(),
                IReadOnlyDictionary<string, object?> target => target.TryGetValue(key, out var value) ? from(value) : undefined(),
                string target when key == "length" => from(target.Length),
                ITsClosedValueCarrier => throw new NotSupportedException("A closed identity carrier does not expose dynamic properties."),
                _ => undefined()
            };
        }

        public T ReadDynamicSlotAs<T>(string key)
        {
            return CastDynamic<T>(ReadDynamicSlot(key));
        }

        public TsValue ReadDynamicSlotOptional(string key)
        {
            return isNullish(_value) ? undefined() : ReadDynamicSlot(key);
        }

        public TsValue WriteDynamicSlot(string key, object? value)
        {
            var stored = from(value);
            switch (unwrapForOperation(_value))
            {
                case Undefined:
                    throw nullishWriteError(key);
                case null:
                    throw nullishWriteError(key);
                case TsObject target:
                    return target.WriteDynamicSlot(key, stored);
                case TsArray target:
                    return target.WriteDynamicSlot(key, stored);
                case TsFunction target:
                    return target.WriteDynamicSlot(key, stored);
                case IDynamicObject target:
                    target.WriteDynamicSlot(key, stored.unwrap());
                    return stored;
                case IDictionary<string, object?> target:
                    target[key] = stored.unwrap();
                    return stored;
                default:
                    throw new TypeError($"Cannot set property '{key}' on a closed non-object JavaScript carrier.");
            }
        }

        public TsValue ReadDynamicElement(object? key)
        {
            return ReadDynamicSlot(propertyKey(key));
        }

        public TsValue ReadDynamicElementOptional(Func<object?> key)
        {
            return isNullish(_value) ? undefined() : ReadDynamicElement(key());
        }

        public TsValue WriteDynamicElement(object? key, object? value)
        {
            return WriteDynamicSlot(propertyKey(key), value);
        }

        public TsValue InvokeDynamic(params object?[] arguments)
        {
            return invokeDynamicWithThis(undefined(), arguments);
        }

        public TsValue InvokeDynamicOptional(Func<object?[]> arguments)
        {
            return isNullish(_value)
                ? undefined()
                : invokeDynamicWithThis(undefined(), arguments());
        }

        public TsValue InvokeDynamicSlot(
            string key,
            bool optionalReceiver,
            bool optionalCall,
            Func<object?[]> arguments)
        {
            if (optionalReceiver && isNullish(_value))
            {
                return undefined();
            }
            var callee = ReadDynamicSlot(key);
            if (optionalCall && isNullish(callee))
            {
                return undefined();
            }
            return callee.invokeDynamicWithThis(this, arguments());
        }

        public TsValue InvokeDynamicElement(
            Func<object?> key,
            bool optionalReceiver,
            bool optionalCall,
            Func<object?[]> arguments)
        {
            if (optionalReceiver && isNullish(_value))
            {
                return undefined();
            }
            var callee = ReadDynamicElement(key());
            if (optionalCall && isNullish(callee))
            {
                return undefined();
            }
            return callee.invokeDynamicWithThis(this, arguments());
        }

        private TsValue invokeDynamicWithThis(
            TsValue receiver,
            object?[] arguments)
        {
            return unwrapForOperation(_value) is TsFunction target
                ? target.InvokeDynamicWithThis(receiver, arguments)
                : throw new TypeError("Value is not callable.");
        }

        public TsValue ConstructDynamic(params object?[] arguments)
        {
            return unwrapForOperation(_value) is TsFunction target
                ? target.ConstructDynamic(arguments)
                : throw new TypeError("Value is not a constructor.");
        }

        public static TsValue ApplyDynamicBinary(object? left, string op, object? right)
        {
            return op switch
            {
                "+" => plus(left, right),
                "-" or "*" or "/" or "%" => from(NativeNumbers.Binary(unwrapForOperation(left), op, unwrapForOperation(right))),
                _ => throw unsupportedOperator(op)
            };
        }

        public static TsValue ApplyDynamicLogical(object? left, string op, Func<object?> right)
        {
            return op switch
            {
                "??" => isNullish(left) ? from(right()) : from(left),
                "&&" => truthy(left) ? from(right()) : from(left),
                "||" => truthy(left) ? from(left) : from(right()),
                _ => throw unsupportedOperator(op)
            };
        }

        public static bool ApplyDynamicBinaryBoolean(object? left, string op, object? right)
        {
            return op switch
            {
                "==" => looseEquals(left, right),
                "!=" => !looseEquals(left, right),
                "===" => strictEquals(left, right),
                "!==" => !strictEquals(left, right),
                "<" => tryCompare(left, right, out var lessThanComparison) && lessThanComparison < 0,
                "<=" => tryCompare(left, right, out var lessThanOrEqualComparison) && lessThanOrEqualComparison <= 0,
                ">" => tryCompare(left, right, out var greaterThanComparison) && greaterThanComparison > 0,
                ">=" => tryCompare(left, right, out var greaterThanOrEqualComparison) && greaterThanOrEqualComparison >= 0,
                _ => throw unsupportedOperator(op)
            };
        }

        public static TsValue ApplyDynamicUnary(object? operand, string op)
        {
            return op switch
            {
                "+" or "-" or "~" => from(NativeNumbers.Unary(unwrapForOperation(operand), op)),
                _ => throw unsupportedOperator(op)
            };
        }

        public static bool ApplyDynamicUnaryBoolean(object? operand, string op)
        {
            return op switch
            {
                "!" => !truthy(operand),
                _ => throw unsupportedOperator(op)
            };
        }

        public static TsValue ApplyDynamicVoid(object? operand)
        {
            _ = operand;
            return undefined();
        }

        public static string ApplyDynamicTypeof(object? operand)
        {
            var unwrapped = unwrapForOperation(operand);
            return unwrapped switch
            {
                Undefined => "undefined",
                null => "object",
                bool => "boolean",
                string => "string",
                double or float or decimal or Half or int or long or uint or ulong or byte or sbyte or short or ushort or nint or nuint or Int128 or UInt128 => "number",
                TsFunction => "function",
                _ => "object"
            };
        }

        public static bool ToDynamicBoolean(object? value)
        {
            return truthy(value);
        }

        public static bool IsDynamicInstanceOf<T>(object? value)
        {
            return UnwrapDynamicCarrier(value) is T;
        }

        public static T CastDynamic<T>(TsValue value) => CastDynamic<T>(value._value);

        public static T CastDynamic<T>(object? value)
        {
            if (TryCastDynamic<T>(value, out var typed))
            {
                return typed;
            }
            var unwrapped = unwrapForOperation(value);
            if (unwrapped is null or Undefined)
            {
                throw new TypeError("Cannot cast null or undefined to the requested closed value carrier.");
            }
            throw new TypeError("TsValue cannot cross the requested typed boundary because the closed carrier value is not assignable.");
        }

        internal static bool TryCastDynamic<T>(object? value, out T result)
        {
            var carrier = UnwrapDynamicCarrier(value);
            if (carrier is T direct)
            {
                result = direct;
                return true;
            }
            var unwrapped = unwrapForOperation(carrier);
            if (unwrapped is T typed)
            {
                result = typed;
                return true;
            }
            if (NativeNumbers.TryConvert(unwrapped, out result))
            {
                return true;
            }
            if ((unwrapped is null or Undefined) && default(T) is null)
            {
                result = default!;
                return true;
            }
            result = default!;
            return false;
        }

        private static bool isSupported(object? value)
        {
            return value switch
            {
                null => true,
                bool => true,
                string => true,
                double => true,
                float => true,
                decimal => true,
                int => true,
                long => true,
                uint => true,
                ulong => true,
                byte => true,
                sbyte => true,
                short => true,
                ushort => true,
                Half or nint or nuint or Int128 or UInt128 => true,
                TsObject => true,
                TsArray => true,
                TsUnion => true,
                TsFunction => true,
                IDynamicObject => true,
                Error => true,
                Exception => true,
                Undefined => true,
                IDictionary<string, object?> => true,
                IReadOnlyDictionary<string, object?> => true,
                ITsClosedValueCarrier => true,
                _ => false
            };
        }

        private static TsValue plus(object? left, object? right)
        {
            var leftValue = unwrapForOperation(left);
            var rightValue = unwrapForOperation(right);
            return leftValue is string || rightValue is string
                ? from(toJsString(leftValue) + toJsString(rightValue))
                : from(NativeNumbers.Binary(leftValue, "+", rightValue));
        }

        private static bool tryCompare(object? left, object? right, out int comparison)
        {
            var leftValue = unwrapForOperation(left);
            var rightValue = unwrapForOperation(right);
            if (leftValue is string leftText && rightValue is string rightText)
            {
                comparison = string.CompareOrdinal(leftText, rightText);
                return true;
            }
            var result = NativeNumbers.Compare(leftValue, rightValue);
            comparison = result.GetValueOrDefault();
            return result.HasValue;
        }

        private static bool looseEquals(object? left, object? right)
        {
            var leftValue = unwrapForOperation(left);
            var rightValue = unwrapForOperation(right);
            if (isNullish(leftValue) && isNullish(rightValue))
            {
                return true;
            }
            if (isNullish(leftValue) || isNullish(rightValue))
            {
                return false;
            }
            return strictEquals(leftValue, rightValue);
        }

        private static bool strictEquals(object? left, object? right)
        {
            var leftValue = unwrapForOperation(left);
            var rightValue = unwrapForOperation(right);
            if (NativeNumbers.IsNumber(leftValue) && NativeNumbers.IsNumber(rightValue))
                return NativeNumbers.Compare(leftValue, rightValue) == 0;
            if (ReferenceEquals(leftValue, rightValue))
            {
                return true;
            }
            if (isNullish(leftValue) || isNullish(rightValue))
            {
                return false;
            }
            return leftValue!.GetType() == rightValue!.GetType() && Equals(leftValue, rightValue);
        }

        private static bool truthy(object? value)
        {
            var unwrapped = unwrapForOperation(value);
            return unwrapped switch
            {
                null => false,
                Undefined => false,
                bool boolean => boolean,
                string text => text.Length > 0,
                double number => number != 0 && !double.IsNaN(number),
                float number => number != 0 && !float.IsNaN(number),
                decimal number => number != 0,
                int number => number != 0,
                long number => number != 0,
                uint number => number != 0,
                ulong number => number != 0,
                byte number => number != 0,
                sbyte number => number != 0,
                short number => number != 0,
                ushort number => number != 0,
                Half number => number != (Half)0 && !Half.IsNaN(number),
                nint number => number != 0,
                nuint number => number != 0,
                Int128 number => number != 0,
                UInt128 number => number != 0,
                _ => true
            };
        }

        private static string toJsString(object? value)
        {
            var unwrapped = unwrapForOperation(value);
            return unwrapped switch
            {
                null => "null",
                Undefined => "undefined",
                bool boolean => boolean ? "true" : "false",
                string text => text,
                double number => number.ToString(CultureInfo.InvariantCulture),
                float number => number.ToString(CultureInfo.InvariantCulture),
                decimal number => number.ToString(CultureInfo.InvariantCulture),
                int number => number.ToString(CultureInfo.InvariantCulture),
                long number => number.ToString(CultureInfo.InvariantCulture),
                uint number => number.ToString(CultureInfo.InvariantCulture),
                ulong number => number.ToString(CultureInfo.InvariantCulture),
                byte number => number.ToString(CultureInfo.InvariantCulture),
                sbyte number => number.ToString(CultureInfo.InvariantCulture),
                short number => number.ToString(CultureInfo.InvariantCulture),
                ushort number => number.ToString(CultureInfo.InvariantCulture),
                Half number => number.ToString(CultureInfo.InvariantCulture),
                nint number => number.ToString(CultureInfo.InvariantCulture),
                nuint number => number.ToString(CultureInfo.InvariantCulture),
                Int128 number => number.ToString(CultureInfo.InvariantCulture),
                UInt128 number => number.ToString(CultureInfo.InvariantCulture),
                _ => "[object Object]"
            };
        }

        private static TsValue readErrorSlot(Error error, string key)
        {
            return key switch
            {
                "name" => from(error.name),
                "message" => from(error.message),
                "stack" => error.stack is null ? undefined() : from(error.stack),
                _ => undefined()
            };
        }

        private static TsValue readExceptionSlot(Exception error, string key)
        {
            return key switch
            {
                "name" => from("Error"),
                "message" => from(error.Message),
                "stack" => error.StackTrace is null ? undefined() : from(error.StackTrace),
                _ => undefined()
            };
        }

        private static bool isNullish(object? value)
        {
            var unwrapped = unwrapForOperation(value);
            return unwrapped is null or Undefined;
        }

        internal static object? UnwrapDynamicCarrier(object? value)
        {
            var unwrapped = value is TsValue typed ? typed._value : value;
            if (!isSupported(unwrapped))
            {
                throw closedCarrierError();
            }
            return unwrapped;
        }

        private static object? unwrapForOperation(object? value)
        {
            var carrier = UnwrapDynamicCarrier(value);
            return carrier is TsUnion union ? unwrapForOperation(union.value()) : carrier;
        }

        private static NotSupportedException unsupportedOperator(string op)
        {
            return new NotSupportedException($"TsValue has no closed native operation for '{op}'.");
        }

        internal static string propertyKey(object? key)
        {
            key = unwrapForOperation(key);
            return key switch
            {
                Undefined => "undefined",
                null => "null",
                string value => value,
                bool value => value ? "true" : "false",
                byte value => value.ToString(CultureInfo.InvariantCulture),
                sbyte value => value.ToString(CultureInfo.InvariantCulture),
                short value => value.ToString(CultureInfo.InvariantCulture),
                ushort value => value.ToString(CultureInfo.InvariantCulture),
                int value => value.ToString(CultureInfo.InvariantCulture),
                uint value => value.ToString(CultureInfo.InvariantCulture),
                long value => value.ToString(CultureInfo.InvariantCulture),
                ulong value => value.ToString(CultureInfo.InvariantCulture),
                float value => value.ToString(CultureInfo.InvariantCulture),
                double value => value.ToString(CultureInfo.InvariantCulture),
                decimal value => value.ToString(CultureInfo.InvariantCulture),
                _ => throw new TypeError("Only closed primitive property keys are supported by TsValue.")
            };
        }

        private static NotSupportedException closedCarrierError()
        {
            return new NotSupportedException("TsValue requires a closed TypeScript runtime carrier.");
        }

        private static TypeError nullishReadError(string key)
        {
            return new TypeError($"Cannot read property '{key}' of null or undefined.");
        }

        private static TypeError nullishWriteError(string key)
        {
            return new TypeError($"Cannot set property '{key}' of null or undefined.");
        }
    }
}
