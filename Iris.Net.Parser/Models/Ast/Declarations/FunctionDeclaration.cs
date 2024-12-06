namespace Iris.Net.Parser.Models.Ast.Declarations;

[Serializable]
public class FunctionDeclaration(string name, FunctionWrapper function) : Node(name)
{
    public FunctionWrapper Function { get; set; } = function;
}