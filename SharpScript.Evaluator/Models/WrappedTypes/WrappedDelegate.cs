namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedDelegate : WrappedEntity
{
    public Delegate Value => (Delegate)Object;

    public WrappedDelegate(Delegate value, string name) : base(value, name)
    {
    }
}