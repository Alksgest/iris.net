namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ConditionalExpression(
    NodeExpression condition,
    Node @true,
    Node? @false = null)
    : NodeExpression(nameof(ConditionalExpression))
{
    public NodeExpression Condition { get; set; } = condition;
    public Node True { get; set; } = @true;
    public Node? False { get; set; } = @false;
}