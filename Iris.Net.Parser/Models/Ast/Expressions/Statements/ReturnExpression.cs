namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ReturnExpression : NodeExpression
{
    public NodeExpression Expression { get; }

    public ReturnExpression(NodeExpression expression) : base(nameof(ReturnExpression))
    {
        Expression = expression;
    }
}