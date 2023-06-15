using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.Models;

public abstract class ObjectInScope
{
    [NestedProperty(Name = "name")] public string Name { get; set; }
    public object Object { get; }

    protected ObjectInScope(object o, string? name)
    {
        Object = o;
        Name = name ?? $"temprorary_value_of_type_{o.GetType()}";
    }
    
    [NestedMethod(Name = "toString")]
    public virtual string ObjectToString()
    {
        return Object.ToString() ?? "null";
    }
}