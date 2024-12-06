namespace Iris.Net.Parser.Models.Ast.Declarations;

[Serializable]
public class VariableDeclaration(string name, Node? value) : Node(name)
{
    public Node? Value { get; } = value;
}