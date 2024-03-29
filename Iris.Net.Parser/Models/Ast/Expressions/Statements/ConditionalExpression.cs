namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ConditionalExpression : NodeExpression
{
    public NodeExpression Condition { get; set; }
    public Node True { get; set; }
    public Node? False { get; set; }

    public ConditionalExpression(
        NodeExpression condition,
        Node @true,
        Node? @false = null) : base(nameof(ConditionalExpression))
    {
        Condition = condition;
        True = @true;
        False = @false;
    }
}