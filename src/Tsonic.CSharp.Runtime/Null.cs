namespace Tsonic.CSharp.Runtime
{
    public sealed class Null : IAbsentValue<Null>
    {
        public static readonly Null value = new();
        public static Null Singleton => value;

        private Null()
        {
        }

        public override string ToString()
        {
            return "null";
        }
    }
}
