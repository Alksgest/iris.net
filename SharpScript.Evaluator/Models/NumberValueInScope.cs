namespace SharpScript.Evaluator.Models;

public class NumberValueInScope : PrimitiveValueInScope<decimal>
{
    public NumberValueInScope(decimal value, string? name = null) : base(value, name)
    {
    }
}