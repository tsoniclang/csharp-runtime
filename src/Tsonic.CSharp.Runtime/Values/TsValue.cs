using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Tsonic.CSharp.Runtime
{
    /// <summary>
    /// Closed value carrier for values whose static TypeScript source type is broad.
    /// </summary>
    public readonly partial struct TsValue
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

            if (value is IClosedUnionValue { IsInitialized: false })
            {
                throw new InvalidOperationException("Union is not initialized.");
            }

            if (!isSupported(value))
            {
                throw new NotSupportedException("TsValue requires a closed TypeScript runtime carrier.");
            }
            return new TsValue(value);
        }

        public static TsValue from<T1, T2>(Union<T1, T2> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2>(Union<T1, T2>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue from<T1, T2, T3>(Union<T1, T2, T3> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2, T3>(Union<T1, T2, T3>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue from<T1, T2, T3, T4>(Union<T1, T2, T3, T4> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2, T3, T4>(Union<T1, T2, T3, T4>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue from<T1, T2, T3, T4, T5>(Union<T1, T2, T3, T4, T5> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2, T3, T4, T5>(Union<T1, T2, T3, T4, T5>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6>(Union<T1, T2, T3, T4, T5, T6> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6>(Union<T1, T2, T3, T4, T5, T6>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7>(Union<T1, T2, T3, T4, T5, T6, T7> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7>(Union<T1, T2, T3, T4, T5, T6, T7>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7, T8>(Union<T1, T2, T3, T4, T5, T6, T7, T8> value)
        {
            return from((object)value);
        }

        public static TsValue from<T1, T2, T3, T4, T5, T6, T7, T8>(Union<T1, T2, T3, T4, T5, T6, T7, T8>? value)
        {
            return value.HasValue ? from(value.Value) : undefined();
        }

        public static TsValue undefined()
        {
            return new TsValue(null);
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
            return unwrapForOperation(_value) is null;
        }

        public bool IsArray()
        {
            return unwrapForOperation(_value) is TsArray or IDynamicArray;
        }

        public int ArrayLength => unwrapForOperation(_value) switch
        {
            TsArray target => target.length,
            IDynamicArray target => target.Length,
            _ => throw new TypeError("A native array length requires an array backing.")
        };

        public TsValue ReadDynamicSlot(string key)
        {
            return unwrapForOperation(_value) switch
            {
                null => throw nullishReadError(key),
                TsObject target => target.ReadDynamicSlot(key),
                TsArray target => target.ReadDynamicSlot(key),
                TsFunction target => target.ReadDynamicSlot(key),
                Error target => readErrorSlot(target, key),
                Exception target => readExceptionSlot(target, key),
                IDynamicObject target => target.TryReadDynamicSlot(key, out var value) ? from(value) : undefined(),
                IDictionary<string, TsValue> target => target.TryGetValue(key, out var value) ? value : undefined(),
                IReadOnlyDictionary<string, TsValue> target => target.TryGetValue(key, out var value) ? value : undefined(),
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
                case IDictionary<string, TsValue> target:
                    target[key] = stored;
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
            return InvokeDynamicWithThis(undefined(), arguments);
        }

        public TsValue InvokeDynamicOptional(Func<object?[]> arguments)
        {
            return isNullish(_value)
                ? undefined()
                : InvokeDynamicWithThis(undefined(), arguments());
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
            return callee.InvokeDynamicWithThis(this, arguments());
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
            return callee.InvokeDynamicWithThis(this, arguments());
        }

        public TsValue InvokeDynamicWithThis(
            TsValue receiver,
            params object?[] arguments)
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

        public static TsValue ApplyDynamicVoid(object? operand)
        {
            _ = operand;
            return undefined();
        }

        public static string ApplyDynamicTypeof(TsValue operand) => ApplyDynamicTypeof(operand._value);

        public static string ApplyDynamicTypeof(object? operand)
        {
            var unwrapped = unwrapForOperation(operand);
            return unwrapped switch
            {
                null => "object",
                bool => "boolean",
                string => "string",
                long or ulong or Int128 or UInt128 => "bigint",
                double or float or decimal or Half or int or uint or byte or sbyte or short or ushort or nint or nuint => "number",
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
            if (unwrapped is null)
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
            if ((unwrapped is null) && default(T) is null)
            {
                result = default!;
                return true;
            }
            result = default!;
            return false;
        }

        internal static bool isSupported<Value>(Value value)
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
                TsValue => true,
                IClosedUnionValue union => union.IsSupported,
                TsFunction => true,
                Task => true,
                IDynamicObject => true,
                Error => true,
                Exception => true,
                IDictionary<string, TsValue> => true,
                IReadOnlyDictionary<string, TsValue> => true,
                IDictionary<string, object?> => true,
                IReadOnlyDictionary<string, object?> => true,
                ITsClosedValueCarrier => true,
                _ => false
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
            return unwrapped is null;
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
            return UnwrapClosedValue(UnwrapDynamicCarrier(value));
        }

        public static object? UnwrapClosedValue(TsValue value) => UnwrapClosedValue(value._value);

        public static object? UnwrapClosedValue(object? value)
        {
            while (true)
            {
                switch (value)
                {
                    case TsValue typed: value = typed._value; break;
                    case IClosedUnionValue union: value = union.UnionValue; break;
                    default: return value;
                }
            }
        }

        internal static string propertyKey(object? key)
        {
            key = unwrapForOperation(key);
            return key switch
            {
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
