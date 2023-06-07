using SharpScript.Parser.Models.Ast.Expressions;

namespace SharpScript.Parser.Models.Ast.Assignments;

public class VariableAssignment : Node
{
    public NodeExpression Value { get; }

    public VariableAssignment(string name, NodeExpression value) : base(name)
    {
        Value = value;
    }
}