namespace SharpScript.Parser.Models.Ast.Expressions.Statements;

public class ReturnExpression : NodeExpression
{
    public NodeExpression Expression { get; }

    public ReturnExpression(NodeExpression expression) : base(nameof(ReturnExpression))
    {
        Expression = expression;
    }
}