namespace Tsonic.CSharp.Runtime;

internal interface IClosedUnionValue
{
    bool IsInitialized { get; }
    object? UnionValue { get; }
}
