namespace Tsonic.CSharp.Runtime;

internal interface IClosedUnionValue
{
    bool IsInitialized { get; }
    bool IsSupported { get; }
    object? UnionValue { get; }
}
