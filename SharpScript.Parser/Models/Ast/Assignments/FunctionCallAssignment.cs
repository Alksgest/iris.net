namespace SharpScript.Parser.Models.Ast.Assignments;

public class FunctionCallAssignment : Node
{
    public FunctionCall Value { get; }

    public FunctionCallAssignment(string name, FunctionCall value) : base(name)
    {
        Value = value;
    }
}