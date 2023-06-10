using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class PropertyExpression : NodeExpression
{
    public VariableExpression Variable { get; set; }
    public NodeExpression? NestedNode { get; set; }

    public PropertyExpression(
        VariableExpression variable,
        NodeExpression? nestedNode = null) : base(nameof(PropertyExpression))
    {
        Variable = variable;
        NestedNode = nestedNode;
    }
}