using Iris.Net.Parser.Models.Ast.Expressions;

namespace Iris.Net.Parser.Models.Ast.Assignments;

[Serializable]
public class VariableAssignment(string name, NodeExpression value) : Node(name)
{
    public NodeExpression Value { get; } = value;
}