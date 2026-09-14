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
        private string? _stack;
        private bool _hasStackOverride;
        private readonly Lazy<string> _capturedStack;

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
            var origin = new StackTrace(1, true);
            _capturedStack = new Lazy<string>(() =>
                (_message.Length == 0 ? name : name + ": " + _message) + "\n" + origin);
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

        public string? stack
        {
            get => _hasStackOverride ? _stack : _capturedStack.Value;
            set
            {
                _stack = value;
                _hasStackOverride = true;
            }
        }

        public override string Message => _message;
    }
}
