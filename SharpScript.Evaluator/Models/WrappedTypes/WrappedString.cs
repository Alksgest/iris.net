using SharpScript.Evaluator.Attributes.Objects;

namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedString: WrappedPrimitive<string> 
{
    [NestedProperty("length")] public decimal Length => Value.Length;
    
    public WrappedString(string value, string? name = null) : base(value, name)
    {
    }
}