namespace Iris.Net.Parser.Models.Ast.Expressions;

/// <summary>
/// Represents binary expression e.g. "2 + 2"
/// </summary>
[Serializable]
public class BinaryExpression(
    NodeExpression left,
    string op,
    NodeExpression right)
    : UnaryExpression(left, op, nameof(BinaryExpression))
{
    public NodeExpression Right { get; } = right;
}