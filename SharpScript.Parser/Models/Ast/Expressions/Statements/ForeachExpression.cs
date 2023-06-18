using SharpScript.Parser.Models.Ast.Expressions.EmbeddedTypes;

namespace SharpScript.Parser.Models.Ast.Expressions.Statements;

public class ForeachExpression : NodeExpression
{
    public VariableExpression VariableExpression { get; set; }
    public NodeExpression Iterable { get; set; }
    public BreakableScopedNode Body { get; set; }
    
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