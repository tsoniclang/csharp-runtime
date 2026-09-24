namespace Tsonic.CSharp.Runtime
{
    public sealed class Undefined : IAbsentValue<Undefined>
    {
        public static readonly Undefined value = new();
        public static Undefined Singleton => value;

        private Undefined()
        {
        }

        public override string ToString()
        {
            return "undefined";
        }
    }
}
