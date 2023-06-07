namespace SharpScript.Parser.Models.Ast.Expressions;

public class BinaryExpression : NodeExpression
{
    public NodeExpression Left { get; }
    public string Operator { get; }
    public NodeExpression Right { get; }

    public BinaryExpression(NodeExpression left, string op, NodeExpression right) : base(nameof(BinaryExpression))
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}