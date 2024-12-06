namespace Iris.Net.Evaluator.Models.WrappedTypes;

public abstract class WrappedPrimitive<T>(T value, string? name = null) : WrappedEntity(value, name)
    where T : notnull
{
    public T Value => (T)Object;
}