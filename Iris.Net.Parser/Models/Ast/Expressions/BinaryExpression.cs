namespace Iris.Net.Parser.Models.Ast.Expressions;

/// <summary>
/// Represents binary expression e.g. "2 + 2"
/// </summary>
[Serializable]
public class BinaryExpression : UnaryExpression
{
    public NodeExpression Right { get; }

    public BinaryExpression(
        NodeExpression left,
        string op,
        NodeExpression right) : base(left, op, nameof(BinaryExpression))
    {
        Right = right;
    }
}