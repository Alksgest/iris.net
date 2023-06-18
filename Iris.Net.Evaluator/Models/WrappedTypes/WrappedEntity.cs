using Iris.Net.Evaluator.Attributes.Objects;

namespace Iris.Net.Evaluator.Models.WrappedTypes;

public abstract class WrappedEntity
{
    [NestedProperty("name")] public string Name { get; set; }
    public object Object { get; }

    protected WrappedEntity(object o, string? name)
    {
        Object = o;
        Name = name ?? $"temporary_value_of_type_{o.GetType()}";
    }
    
    [NestedMethod("toString")]
    public virtual string ObjectToString()
    {
        return Object.ToString() ?? "null";
    }
}