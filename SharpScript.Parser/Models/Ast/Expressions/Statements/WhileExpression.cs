namespace SharpScript.Parser.Models.Ast.Expressions.Statements;

public class WhileExpression : NodeExpression
{
    public NodeExpression Condition { get; set; }
    public BreakableScopedNode Body { get; set; }

    public WhileExpression(NodeExpression condition, BreakableScopedNode body) : base(nameof(WhileExpression))
    {
        Condition = condition;
        Body = body;
    }
}