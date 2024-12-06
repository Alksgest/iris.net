namespace Iris.Net.Parser.Models.Ast.Expressions;

/// <summary>
/// Represents unary expression e.g. "-1" or "!true"
/// </summary>
[Serializable]
public class UnaryExpression(
    NodeExpression expression,
    string op,
    string? name = null)
    : NodeExpression(name ?? nameof(UnaryExpression))
{
    public NodeExpression Left { get; } = expression;
    public string Operator { get; } = op;
}