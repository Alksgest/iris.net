namespace SharpScript.Parser.Models.Ast.Expressions;

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