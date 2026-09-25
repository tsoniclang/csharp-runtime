using System;
using System.Diagnostics;

namespace Tsonic.CSharp.Runtime
{
    /// <summary>
    /// JavaScript-style Error base type with lowercase property aliases.
    /// </summary>
    public class Error : Exception
    {
        private string _name = nameof(Error);
        private string _message = string.Empty;

        public Error()
            : this(null, null)
        {
        }

        public Error(string? message)
            : this(message, null)
        {
        }

        public Error(string? message, Exception? innerException)
            : base(message, innerException)
        {
            _message = message ?? string.Empty;
        }

        public static void captureStackTrace(Error error)
        {
            ArgumentNullException.ThrowIfNull(error);
            var origin = new StackTrace(1, true);
            error.stack = (error._message.Length == 0 ? error.name : error.name + ": " + error._message) + "\n" + origin;
        }

        public virtual string name
        {
            get => _name;
            set => _name = value;
        }

        public string message
        {
            get => _message;
            set => _message = value;
        }

        public string? stack { get; set; }

        public override string Message => _message;
    }
}
