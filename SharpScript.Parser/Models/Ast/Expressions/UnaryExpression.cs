namespace SharpScript.Parser.Models.Ast.Expressions;

public class UnaryExpression : NodeExpression
{
    public NodeExpression Left { get; }
    public string Operator { get; }

    public UnaryExpression(
        NodeExpression expression,
        string op,
        string? name = null) : base(name ?? nameof(UnaryExpression))
    {
        Left = expression;
        Operator = op;
    }
}