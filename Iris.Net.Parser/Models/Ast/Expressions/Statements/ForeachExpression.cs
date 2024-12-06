using Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ForeachExpression(
    VariableExpression variableExpression,
    NodeExpression iterable,
    BreakableScopedNode body)
    : NodeExpression(nameof(ForeachExpression))
{
    public VariableExpression VariableExpression { get; } = variableExpression;
    public NodeExpression Iterable { get; } = iterable;
    public BreakableScopedNode Body { get; } = body;
}