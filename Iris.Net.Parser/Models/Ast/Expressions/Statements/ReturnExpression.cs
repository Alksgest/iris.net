namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ReturnExpression(NodeExpression expression) : NodeExpression(nameof(ReturnExpression))
{
    public NodeExpression Expression { get; } = expression;
}