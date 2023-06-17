namespace SharpScript.Evaluator.Models.WrappedTypes;

public abstract class WrappedPrimitive<T> : WrappedEntity where T : notnull
{
    public T Value => (T)Object;

    public WrappedPrimitive(T value, string? name = null) : base(value, name)
    {
    }
}