using SharpScript.Parser.Models.Ast.Expressions.Functions;

namespace SharpScript.Parser.Models.Ast.Assignments;

public class FunctionCallAssignment : Node
{
    public FunctionCallExpression Value { get; }

    public FunctionCallAssignment(string name, FunctionCallExpression value) : base(name)
    {
        Value = value;
    }
}