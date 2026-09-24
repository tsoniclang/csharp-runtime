using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Tsonic.CSharp.Runtime;

public readonly struct Never
{
    [DoesNotReturn]
    public T Value<T>() => throw new UnreachableException("A non-returning source operation returned a value.");
}
