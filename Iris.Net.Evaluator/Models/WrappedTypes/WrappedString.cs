using Iris.Net.Evaluator.Attributes.Objects;

namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedString: WrappedPrimitive<string> 
{
    [NestedProperty("length")] public decimal Length => Value.Length;
    
    public WrappedString(string value, string? name = null) : base(value, name)
    {
    }
}