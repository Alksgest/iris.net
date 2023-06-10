namespace SharpScript.Evaluator.Models;

public class PrimitiveValueInScope<T> : ObjectInScope where T : notnull
{
    public T Value => (T)Object;

    public PrimitiveValueInScope(T value) : base(value)
    {
    }
}