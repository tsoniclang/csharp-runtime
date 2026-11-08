namespace Tsonic.CSharp.Runtime;

public interface IClosedUnionValue
{
    bool IsInitialized { get; }
    bool IsSupported { get; }
    SourceValueCategory NativeCategory { get; }
    object? UnionValue { get; }
}
