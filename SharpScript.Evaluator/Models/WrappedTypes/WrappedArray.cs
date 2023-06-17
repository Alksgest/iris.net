using SharpScript.Evaluator.Attributes.Objects;

namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedArray : WrappedEntity
{
    public List<object> Value => (List<object>)Object;

    [NestedProperty("length")] public decimal Length => Value.Count;

    public WrappedArray(List<object> value, string? name = null) : base(value, name)
    {
    }

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