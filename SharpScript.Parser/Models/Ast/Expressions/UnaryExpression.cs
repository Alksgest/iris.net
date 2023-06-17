namespace SharpScript.Parser.Models.Ast.Expressions;

/// <summary>
/// Represents unary expression e.g. "-1" or "!true"
/// </summary>
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