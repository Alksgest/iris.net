namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedDelegate(Delegate value, string name) : WrappedEntity(value, name)
{
    public Delegate Value => (Delegate)Object;
}