using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;

namespace Tsonic.CSharp.Runtime
{
    /// <summary>
    /// Closed value carrier for values whose static TypeScript source type is broad.
    /// </summary>
    public readonly partial struct TsValue
    {
        private readonly object? _value;
        public SourceValueCategory Category { get; }

        private TsValue(object? value, SourceValueCategory category)
        {
            _value = value;
            Category = category;
        }

        private static void validateClosedValue(object? value)
        {
            if (value is IClosedUnionValue { IsInitialized: false })
                throw new InvalidOperationException("Union is not initialized.");
            if (!isSupported(value))
                throw new NotSupportedException("TsValue requires a closed TypeScript runtime carrier.");
        }

        private static SourceValueCategory valueCategory(object? value) => value is TsValue stored
            ? stored.Category : value is TsUnion union ? union.value().Category : nativecategory(value);

        public static TsValue undefined()
        {
            return new TsValue(null, SourceValueCategory.Absent);
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
            return fromnative(target);
        }

        public object? unwrap()
        {
            return _value;
        }

        public bool isUndefined()
        {
            return Category == SourceValueCategory.Absent;
        }

        public bool IsArray()
        {
            return UnwrapClosedValue(_value) is TsArray or IDynamicArray or Array;
        }

        public int ArrayLength => UnwrapClosedValue(_value) switch
        {
            TsArray target => target.length,
            IDynamicArray target => target.Length,
            Array target => target.Length,
            _ => throw new TypeError("A native array length requires an array backing.")
        };

        public TsValue ReadDynamicSlot(string key)
        {
            return UnwrapClosedValue(_value) switch
            {
                null => throw nullishReadError(key),
                TsObject target => target.ReadDynamicSlot(key),
                TsArray target => target.ReadDynamicSlot(key),
                TsFunction target => target.ReadDynamicSlot(key),
                Exception target => readExceptionSlot(target, key),
                IDynamicObject target => target.TryReadDynamicSlot(key, out var value) ? fromnative(value) : undefined(),
                IDictionary<string, TsValue> target => target.TryGetValue(key, out var value) ? value : undefined(),
                IReadOnlyDictionary<string, TsValue> target => target.TryGetValue(key, out var value) ? value : undefined(),
                IDictionary<string, object?> target => target.TryGetValue(key, out var value) ? fromnative(value) : undefined(),
                IReadOnlyDictionary<string, object?> target => target.TryGetValue(key, out var value) ? fromnative(value) : undefined(),
                string target when key == "length" => fromnative(target.Length),
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
            return isUndefined() ? undefined() : ReadDynamicSlot(key);
        }

        public TsValue WriteDynamicSlot(string key, object? value)
        {
            return WriteNativeSlot(_value, key, value);
        }

        public TsValue WriteDynamicSlot(string key, TsValue value)
        {
            return WriteNativeSlot(_value, key, value);
        }

        private static TsValue WriteNativeSlot<TValue>(object? receiver, string key, TValue value)
        {
            var stored = fromnative(value);
            switch (unwrapForOperation(receiver))
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
            return isUndefined() ? undefined() : ReadDynamicElement(key());
        }

        public TsValue WriteDynamicElement(object? key, object? value)
        {
            return WriteDynamicSlot(propertyKey(key), value);
        }

        public TsValue WriteDynamicElement(object? key, TsValue value)
        {
            return WriteDynamicSlot(propertyKey(key), value);
        }

        public TsValue InvokeDynamic(params object?[] arguments)
        {
            return InvokeDynamicWithThis(undefined(), arguments);
        }

        public TsValue InvokeDynamicOptional(Func<object?[]> arguments)
        {
            return isUndefined()
                ? undefined()
                : InvokeDynamicWithThis(undefined(), arguments());
        }

        public TsValue InvokeDynamicSlot(
            string key,
            bool optionalReceiver,
            bool optionalCall,
            Func<object?[]> arguments)
        {
            if (optionalReceiver && isUndefined())
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
            if (optionalReceiver && isUndefined())
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
            return UnwrapClosedValue(_value) is TsFunction target
                ? target.InvokeDynamicWithThis(receiver, arguments)
                : throw new TypeError("Value is not callable.");
        }

        public TsValue ConstructDynamic(params object?[] arguments)
        {
            return UnwrapClosedValue(_value) is TsFunction target
                ? target.ConstructDynamic(arguments)
                : throw new TypeError("Value is not a constructor.");
        }

        public static TsValue ApplyDynamicVoid(object? operand)
        {
            _ = operand;
            return undefined();
        }

        public static string ApplyDynamicTypeof(TsValue operand) => categoryName(operand.Category);

        public static string ApplyDynamicTypeof(object? operand) => categoryName(nativecategory(operand));

        private static string categoryName(SourceValueCategory category) => category switch
        {
            SourceValueCategory.Absent or SourceValueCategory.Object => "object",
            SourceValueCategory.Boolean => "boolean",
            SourceValueCategory.Number => "number",
            SourceValueCategory.BigInt => "bigint",
            SourceValueCategory.String => "string",
            SourceValueCategory.Symbol => "symbol",
            SourceValueCategory.Function => "function",
            _ => throw new InvalidOperationException("The closed value has no native category."),
        };

        public static bool ToDynamicBoolean(object? value)
        {
            return truthy(value);
        }

        public static bool IsDynamicInstanceOf<T>(object? value)
        {
            return UnwrapDynamicCarrier(value) is T;
        }

        public static T CastDynamic<T>(TsValue value)
        {
            if (TryCastCarrier(value._value, out T result)) return result;
            throw new TypeError(value.isUndefined() ? "Cannot cast null or undefined to the requested closed value carrier."
                : "TsValue cannot cross the requested typed boundary because the closed carrier value is not assignable.");
        }

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
            return TryCastCarrier(UnwrapDynamicCarrier(value), out result);
        }

        private static bool TryCastCarrier<T>(object? carrier, out T result)
        {
            if (carrier is T direct)
            {
                result = direct;
                return true;
            }
            var unwrapped = UnwrapClosedValue(carrier);
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

        public static bool isSupported<Value>(Value value)
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
                Half or nint or nuint or Int128 or UInt128 or BigInteger => true,
                TsObject => true,
                TsArray => true,
                TsUnion => true,
                TsValue => true,
                IClosedUnionValue union => union.IsSupported,
                TsFunction or Delegate => true,
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

        private static TsValue readExceptionSlot(Exception error, string key)
        {
            return key switch
            {
                "name" => fromnative(error.GetType().Name),
                "message" => fromnative(error.Message),
                "stack" => fromnative(error.StackTrace),
                "cause" => fromnative(error.InnerException),
                _ => undefined()
            };
        }

        private static bool isNullish(object? value)
        {
            if (value is TsValue stored) return stored.Category == SourceValueCategory.Absent;
            var unwrapped = unwrapForOperation(value);
            return unwrapped is null;
        }

        internal static object? UnwrapDynamicCarrier(object? value)
        {
            if (value is TsValue typed) return typed._value;
            var unwrapped = value;
            if (!isSupported(unwrapped))
            {
                throw closedCarrierError();
            }
            return unwrapped;
        }

        private static object? unwrapForOperation(object? value)
        {
            if (value is TsValue stored && stored.Category is SourceValueCategory.Object or SourceValueCategory.Function or SourceValueCategory.Symbol)
                return stored._value;
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
