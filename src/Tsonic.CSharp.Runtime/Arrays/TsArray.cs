using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tsonic.CSharp.Runtime
{
    public sealed class TsArray : IDynamicArray
    {
        private readonly List<Slot> _values = new();

        private readonly struct Slot
        {
            private readonly TsValue _value;

            private Slot(bool isPresent, TsValue value)
            {
                IsPresent = isPresent;
                _value = value;
            }

            public bool IsPresent { get; }

            public TsValue Value => IsPresent ? _value : TsValue.undefined();

            public static Slot Hole => default;

            public static Slot Present(TsValue value)
            {
                return new Slot(true, value);
            }
        }

        public TsArray()
        {
        }

        public TsArray(IEnumerable<object?> source)
        {
            foreach (var value in source)
            {
                _values.Add(Slot.Present(TsValue.from(value)));
            }
        }

        public int length => _values.Count;

        public TsValue ReadDynamicSlot(string key)
        {
            return key == "length" ? TsValue.from(length) : ReadDynamicElement(key);
        }

        public TsValue WriteDynamicSlot(string key, object? value)
        {
            if (key == "length")
            {
                Resize(toArrayIndex(value));
                return TsValue.from(length);
            }
            return WriteDynamicElement(key, value);
        }

        public TsValue ReadDynamicElement(object? key)
        {
            var index = toArrayIndex(key);
            return index >= 0 && index < _values.Count ? _values[index].Value : TsValue.undefined();
        }

        public TsValue WriteDynamicElement(object? key, object? value)
        {
            return WriteAt(toArrayIndex(key), value);
        }

        private TsValue WriteAt(int index, object? value)
        {
            if (index < 0)
            {
                throw new RangeError("Array index cannot be negative.");
            }
            while (_values.Count <= index)
            {
                _values.Add(Slot.Hole);
            }
            var stored = TsValue.from(value);
            _values[index] = Slot.Present(stored);
            return stored;
        }

        int IDynamicArray.Length => length;

        bool IDynamicArray.HasOwn(string key) => key == "length" ||
            int.TryParse(key, out var index) && HasIndex(index);

        IEnumerable<KeyValuePair<string, object?>> IDynamicArray.Entries() => entries();

        bool IDynamicArray.HasIndex(int index) => HasIndex(index);

        private bool HasIndex(int index) => index >= 0 && index < _values.Count && _values[index].IsPresent;

        bool IDynamicArray.TryGetAt(int index, out object? value) => TryGetAt(index, out value);

        private bool TryGetAt(int index, out object? value)
        {
            if (!HasIndex(index)) { value = null; return false; }
            value = _values[index].Value.unwrap();
            return true;
        }

        bool IDynamicArray.TrySetAt(int index, object? value)
        {
            if (index < 0) return false;
            WriteAt(index, value);
            return true;
        }

        int IDynamicArray.SetLength(int newLength)
        {
            Resize(newLength);
            return length;
        }

        bool IDynamicArray.DeleteAt(int index)
        {
            if (index >= 0 && index < _values.Count) _values[index] = Slot.Hole;
            return true;
        }

        bool IDynamicObject.TryReadDynamicSlot(string key, out object? value)
        {
            if (key == "length") { value = length; return true; }
            value = null;
            return int.TryParse(key, out var index) && TryGetAt(index, out value);
        }

        void IDynamicObject.WriteDynamicSlot(string key, object? value) => WriteDynamicSlot(key, value);

        public IEnumerable<KeyValuePair<string, object?>> entries()
        {
            for (var index = 0; index < _values.Count; index++)
            {
                var slot = _values[index];
                if (!slot.IsPresent)
                {
                    continue;
                }
                yield return new KeyValuePair<string, object?>(index.ToString(CultureInfo.InvariantCulture), slot.Value.unwrap());
            }
        }

        private void Resize(int length)
        {
            if (length < 0)
            {
                throw new RangeError("Invalid array length.");
            }
            if (length < _values.Count)
            {
                _values.RemoveRange(length, _values.Count - length);
                return;
            }
            while (_values.Count < length)
            {
                _values.Add(Slot.Hole);
            }
        }

        private static int toArrayIndex(object? value)
        {
            var key = TsValue.propertyKey(value);
            return int.TryParse(key, out var index) ? index : throw new TypeError("Array element access requires a numeric index.");
        }
    }
}
