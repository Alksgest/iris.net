namespace Iris.Net.Parser.Models.Ast.Declarations;

public class FunctionDeclaration : Node
{
    public FunctionWrapper Function { get; set; }

    public FunctionDeclaration(string name, FunctionWrapper function) : base(name)
    {
        Function = function;
    }
}