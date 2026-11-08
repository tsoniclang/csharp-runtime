namespace Tsonic.CSharp.Runtime;

public interface IClosedUnionValue
{
    bool IsInitialized { get; }
    bool IsSupported { get; }
    object? UnionValue { get; }
}
