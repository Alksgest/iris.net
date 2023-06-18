namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedNumber : WrappedPrimitive<decimal>
{
    public WrappedNumber(decimal value, string? name = null) : base(value, name)
    {
    }
}