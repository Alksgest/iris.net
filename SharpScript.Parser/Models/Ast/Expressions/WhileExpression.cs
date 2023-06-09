namespace SharpScript.Parser.Models.Ast.Expressions;

public class WhileExpression : NodeExpression
{
    public NodeExpression Condition { get; set; }
    public ScopedNode Body { get; set; }

    public WhileExpression(NodeExpression condition, ScopedNode body) : base(nameof(WhileExpression))
    {
        Condition = condition;
        Body = body;
    }
}