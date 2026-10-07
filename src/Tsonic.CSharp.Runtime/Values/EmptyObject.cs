namespace Tsonic.CSharp.Runtime;

public sealed class EmptyObject : ITsClosedValueCarrier
{
    private bool _frozen;

    public static T Freeze<T>(T value) where T : class
    {
        ((EmptyObject)(object)value)._frozen = true;
        return value;
    }

    public static bool IsFrozen<T>(T value) where T : class => ((EmptyObject)(object)value)._frozen;
}
