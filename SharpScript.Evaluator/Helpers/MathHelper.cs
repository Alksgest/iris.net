namespace SharpScript.Evaluator.Helpers;

public static class MathHelper
{
    internal static decimal UnaryMinus(object operand)
    {
        var value = (decimal)operand;
        return -value;
    }
    internal static decimal Sum(object? left, object? right)
    {
        var (l, r) = ConvertOperands(left, right);
        return l + r;
    }
    
    internal static decimal Subtract(object? left, object? right)
    {
        var (l, r) = ConvertOperands(left, right);
        return l - r;
    }
    
    internal static decimal Multiply(object? left, object? right)
    {
        var (l, r) = ConvertOperands(left, right);
        return l * r;
    }
    
    internal static decimal Divide(object? left, object? right)
    {
        var (l, r) = ConvertOperands(left, right);
        return l / r;
    }

    private static decimal ConvertOperand(object? operand)
    {
        if (operand == null)
        {
            throw new Exception("Operands should not be null");
        }

        return (decimal)operand;
    }

    private static (decimal, decimal) ConvertOperands(object? left, object? right)
    {
        var l = ConvertOperand(left);
        var r = ConvertOperand(right);
        
        return (l, r);
    }
}