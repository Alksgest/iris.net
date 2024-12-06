namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class WhileExpression(NodeExpression condition, BreakableScopedNode body)
    : NodeExpression(nameof(WhileExpression))
{
    public NodeExpression Condition { get; set; } = condition;
    public BreakableScopedNode Body { get; set; } = body;
}