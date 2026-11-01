using System;

namespace Tsonic.CSharp.Runtime;

public sealed class SuppressedError : Exception
{
    public SuppressedError(Exception error, Exception suppressed)
        : base("An error was suppressed during resource disposal.", error)
    {
        Error = error ?? throw new ArgumentNullException(nameof(error));
        Suppressed = suppressed ?? throw new ArgumentNullException(nameof(suppressed));
    }

    public Exception Error { get; }

    public Exception Suppressed { get; }
}
