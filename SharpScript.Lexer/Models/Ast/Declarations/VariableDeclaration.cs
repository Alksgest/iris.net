namespace SharpScript.Lexer.Models.Ast.Declarations;

public class VariableDeclaration : Node
{
    public Node? Value { get; }

    public VariableDeclaration(string name, Node? value) : base(name)
    {
        Value = value;
    }
}