namespace SharpScript.Parser.Models.Ast.Expressions;

public class ConditionalExpression : NodeExpression
{
    public NodeExpression Condition { get; set; }
    public NodeExpression True { get; set; }
    public NodeExpression? False { get; set; }

    public ConditionalExpression(
        string name, NodeExpression condition,
        NodeExpression @true,
        NodeExpression? @false) : base(name)
    {
        Condition = condition;
        True = @true;
        False = @false;
    }
}