namespace Iris.Net.Parser.Models.Ast.Declarations;

[Serializable]
public class VariableDeclaration : Node
{
    public Node? Value { get; }

    public VariableDeclaration(string name, Node? value) : base(name)
    {
        Value = value;
    }
}