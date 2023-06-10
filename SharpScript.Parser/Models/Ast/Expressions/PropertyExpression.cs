using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class PropertyExpression : NodeExpression
{
    //TODO: For now we can call properties only from variables
    public string VariableName { get; set; }
    public Node? NestedNode { get; set; }

    public PropertyExpression(string variableName, Node? nestedNode = null) : base(nameof(PropertyExpression))
    {
        VariableName = variableName;
        NestedNode = nestedNode;
    }
}