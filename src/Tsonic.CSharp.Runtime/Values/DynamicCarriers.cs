using System.Collections.Generic;

namespace Tsonic.CSharp.Runtime
{
    public interface IDynamicObject
    {
        bool TryReadDynamicSlot(string key, out object? value);

        void WriteDynamicSlot(string key, object? value);
    }

    public interface IArrayElementVisitor
    {
        void Visit<TValue>(TValue value);
    }

    public interface IDynamicArray : IDynamicObject
    {
        bool HasOwn(string key);

        IEnumerable<KeyValuePair<string, object?>> Entries();

        int Length { get; }

        bool HasIndex(int index);

        bool TryGetAt(int index, out object? value);

        void VisitElements<TVisitor>(ref TVisitor visitor) where TVisitor : struct, IArrayElementVisitor;

        bool TrySetAt(int index, object? value);

        int SetLength(int newLength);

        bool DeleteAt(int index);
    }
}
