using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.Models;

public class StringValueInScope: PrimitiveValueInScope<string> 
{
    [NestedProperty(Name = "length")] public decimal Length => Value.Length;
    
    public StringValueInScope(string value, string? name = null) : base(value, name)
    {
    }
}