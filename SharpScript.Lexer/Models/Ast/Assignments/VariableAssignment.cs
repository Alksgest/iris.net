using SharpScript.Lexer.Models.Ast.Expressions;

namespace SharpScript.Lexer.Models.Ast.Assignments;

public class VariableAssignment : Node
{
    public NodeExpression Value { get; }

    public VariableAssignment(string name, NodeExpression value) : base(name)
    {
        Value = value;
    }
}