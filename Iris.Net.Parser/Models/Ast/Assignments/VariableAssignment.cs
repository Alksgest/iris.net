using Iris.Net.Parser.Models.Ast.Expressions;

namespace Iris.Net.Parser.Models.Ast.Assignments;

public class VariableAssignment : Node
{
    public NodeExpression Value { get; }

    public VariableAssignment(string name, NodeExpression value) : base(name)
    {
        Value = value;
    }
}