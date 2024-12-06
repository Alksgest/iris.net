using Iris.Net.Evaluator.Attributes.Objects;

namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedString(string value, string? name = null) : WrappedPrimitive<string>(value, name)
{
    [NestedProperty("length")] public decimal Length => Value.Length;
}