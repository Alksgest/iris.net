using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.Models;

public class PrimitiveValueInScope<T> : ObjectInScope where T : notnull
{
    public T Value => (T)Object;

    public PrimitiveValueInScope(T value, string? name = null) : base(value, name)
    {
    }
}

public class StringValueInScope: PrimitiveValueInScope<string> 
{
    [NestedProperty(Name = "length")] public decimal Length => Value.Length;
    
    public StringValueInScope(string value, string? name = null) : base(value, name)
    {
    }
}

public class NumberValueInScope: PrimitiveValueInScope<decimal> 
{
    public NumberValueInScope(decimal value, string? name = null) : base(value, name)
    {
    }
}