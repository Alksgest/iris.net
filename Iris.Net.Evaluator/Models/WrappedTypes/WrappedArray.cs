using Iris.Net.Evaluator.Attributes.Objects;

namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedArray(List<object> value, string? name = null) : WrappedEntity(value, name)
{
    public List<object> Value => (List<object>)Object;

    [NestedProperty("length")] public decimal Length => Value.Count;

    [NestedMethod("get")]
    public object GetElementAt(decimal index)
    {
        var i = (int)index;
        return Value[i];
    }

    [NestedMethod("set")]
    public void SetElementAt(decimal index, object value)
    {
        var i = (int)index;
        Value[i] = value;
    }

    [NestedMethod("add")]
    public void Add(object value)
    {
        Value.Add(value);
    }
    
    [NestedMethod("makeCopy")]
    public List<object> MakeCopy()
    {
        return new (Value);
    }
}