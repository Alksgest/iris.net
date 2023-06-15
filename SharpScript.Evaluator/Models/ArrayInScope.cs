using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.Models;

public class ArrayInScope : ObjectInScope
{
    public List<object> Value => (List<object>)Object;

    [NestedProperty(Name = "length")] public decimal Length => Value.Count;

    public ArrayInScope(List<object> value, string? name = null) : base(value, name)
    {
    }

    [NestedMethod(Name = "get")]
    public object GetElementAt(decimal index)
    {
        var i = (int)index;
        return Value[i];
    }

    [NestedMethod(Name = "set")]
    public void SetElementAt(decimal index, object value)
    {
        var i = (int)index;
        Value[i] = value;
    }

    [NestedMethod(Name = "add")]
    public void Add(object value)
    {
        Value.Add(value);
    }
    
    [NestedMethod(Name = "makeCopy")]
    public List<object> MakeCopy()
    {
        return new List<object>(Value);
    }
}