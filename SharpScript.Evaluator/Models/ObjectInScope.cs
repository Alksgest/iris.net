using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.Models;

public abstract class ObjectInScope
{
    [NestedProperty(Name = "name")] public string Name { get; set; }
    public object Object { get; }

    protected ObjectInScope(object o, string name)
    {
        Object = o;
        Name = name;
    }
}