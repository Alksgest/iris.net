namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedBoolean : WrappedPrimitive<bool>
{
    public WrappedBoolean(bool value, string? name = null) : base(value, name)
    {
    }
}