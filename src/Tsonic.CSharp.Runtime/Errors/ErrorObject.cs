using System;

namespace Tsonic.CSharp.Runtime
{
    public static class ErrorObject
    {
        public static string name(Exception error)
        {
            ArgumentNullException.ThrowIfNull(error);
            return error is Error source ? source.name : error.GetType().Name;
        }

        public static string? stack(Exception error)
        {
            ArgumentNullException.ThrowIfNull(error);
            return error is Error source ? source.stack : error.StackTrace;
        }
    }
}
