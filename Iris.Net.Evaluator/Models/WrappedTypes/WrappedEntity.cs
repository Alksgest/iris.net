using Iris.Net.Evaluator.Attributes.Objects;

namespace Iris.Net.Evaluator.Models.WrappedTypes;

public abstract class WrappedEntity(object o, string? name)
{
    [NestedProperty("name")] public string Name { get; set; } = name ?? $"temporary_value_of_type_{o.GetType()}";
    public object Object { get; } = o;

    [NestedMethod("toString")]
    public virtual string ObjectToString()
    {
        return Object.ToString() ?? "null";
    }
}