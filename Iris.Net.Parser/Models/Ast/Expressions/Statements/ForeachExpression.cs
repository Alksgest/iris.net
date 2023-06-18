using Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ForeachExpression : NodeExpression
{
    public VariableExpression VariableExpression { get; }
    public NodeExpression Iterable { get; }
    public BreakableScopedNode Body { get; }
    
    public ForeachExpression(
        VariableExpression variableExpression,
        NodeExpression iterable,
        BreakableScopedNode body) : base(nameof(ForeachExpression))
    {
        VariableExpression = variableExpression;
        Iterable = iterable;
        Body = body;
    }
}