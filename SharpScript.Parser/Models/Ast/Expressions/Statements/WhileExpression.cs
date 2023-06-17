namespace SharpScript.Parser.Models.Ast.Expressions.Statements;

public class WhileExpression : NodeExpression
{
    public NodeExpression Condition { get; set; }
    public BreakableScopeNode Body { get; set; }

    public WhileExpression(NodeExpression condition, BreakableScopeNode body) : base(nameof(WhileExpression))
    {
        Condition = condition;
        Body = body;
    }
}