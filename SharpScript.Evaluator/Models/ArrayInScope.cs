using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.Models;

public class ArrayInScope : ObjectInScope
{
    public List<object> Value => (List<object>)Object;

    [NestedProperty(Name = "length")] public decimal Length => Value.Count;

    public ArrayInScope(List<object> value) : base(value)
    {
    }

    [NestedMethod(Name = "get")]
    public object ElementAt(decimal index)
    {
        var i = (int)index;
        return Value[i];
    }
}