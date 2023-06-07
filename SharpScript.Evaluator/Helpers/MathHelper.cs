namespace SharpScript.Evaluator.Helpers;

public static class MathHelper
{
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

    private static (decimal, decimal) ConvertOperands(object? left, object? right)
    {
        if (left == null || right == null)
        {
            throw new Exception("Operands should not be null");
        }

        var l = (decimal)left;
        var r = (decimal)right;
        return (l, r);
    }
}