using SharpScript.Evaluator.Models;

namespace SharpScript.Evaluator.Helpers;

public static class OperatorsHelper
{
    internal static decimal UnaryMinus(object operand)
    {
        var value = ConvertOperand<decimal>(operand);
        return -value;
    }

    internal static bool LogicalNot(object operand)
    {
        var value = ConvertOperand<bool>(operand);
        return !value;
    }
    
    internal static bool LogicalOr(object? left, object? right)
    {
        var (l, r) = ConvertOperands<bool>(left, right);
        return l || r;
    }
    
    internal static bool LogicalAnd(object? left, object? right)
    {
        var (l, r) = ConvertOperands<bool>(left, right);
        return l && r;
    }

    internal static decimal Reminder(object? left, object? right)
    {
        var (l, r) = ConvertOperands<decimal>(left, right);
        return l % r;
    }

    internal static bool GreaterThen(object? left, object? right, bool orEqual = false)
    {
        if (left?.GetType() != right?.GetType())
        {
            throw new ArgumentException($"Types {left?.GetType()} and {right?.GetType()} are not comparable");
        }
        
        if (left == null && right == null)
        {
            return false;
        }

        if (left == null || right == null)
        {
            return false;
        }
        
        if (left is not IComparable l || right is not IComparable r)
        {
            throw new ArgumentException($"It is impossible to compare {left?.GetType()} with {right?.GetType()}");
        }
        
        var res = l.CompareTo(r);
        return res == 1 || (orEqual && res == 0);
    }

    internal static bool LessThen(object? left, object? right,  bool orEqual = false)
    {
        if (left?.GetType() != right?.GetType())
        {
            throw new ArgumentException($"Types {left?.GetType()} and {right?.GetType()} are not comparable");
        }

        if (left == null && right == null)
        {
            return false;
        }

        if (left == null || right == null)
        {
            return false;
        }

        if (left is not IComparable l || right is not IComparable r)
        {
            throw new ArgumentException($"It is impossible to compare {left?.GetType()} with {right?.GetType()}");
        }

        var res = l.CompareTo(r);
        return res == -1 || (orEqual && res == 0);
    }

    internal static bool Equal(object? left, object? right)
    {
        if (left?.GetType() != right?.GetType())
        {
            throw new ArgumentException($"Types {left?.GetType()} and {right?.GetType()} are not comparable");
        }

        if (left == null && right == null)
        {
            return true;
        }

        if (left == null || right == null)
        {
            return false;
        }

        if (left is not IComparable l || right is not IComparable r)
        {
            return left.Equals(right);
        }

        var res = l.CompareTo(r);
        return res == 0;
    }
    
    internal static bool NotEqual(object? left, object? right)
    {
        if (left?.GetType() != right?.GetType())
        {
            throw new ArgumentException($"Types {left?.GetType()} and {right?.GetType()} are not comparable");
        }

        if (left == null && right == null)
        {
            return false;
        }

        if (left == null || right == null)
        {
            return true;
        }

        if (left is not IComparable l || right is not IComparable r)
        {
            return !left.Equals(right);
        }

        var res = l.CompareTo(r);
        return res != 0;
    }

    internal static decimal Sum(object? left, object? right)
    {
        var (l, r) = ConvertOperands<decimal>(left, right);
        return l + r;
    }

    internal static decimal Subtract(object? left, object? right)
    {
        var (l, r) = ConvertOperands<decimal>(left, right);
        return l - r;
    }

    internal static decimal Multiply(object? left, object? right)
    {
        var (l, r) = ConvertOperands<decimal>(left, right);
        return l * r;
    }

    internal static decimal Divide(object? left, object? right)
    {
        var (l, r) = ConvertOperands<decimal>(left, right);
        return l / r;
    }

    private static T ConvertOperand<T>(object? operand)
    {
        if (operand == null)
        {
            throw new Exception("Operands should not be null");
        }

        return (T)operand;
    }

    private static (T, T) ConvertOperands<T>(object? left, object? right)
    {
        var l = ConvertOperand<T>(left);
        var r = ConvertOperand<T>(right);

        return (l, r);
    }
}