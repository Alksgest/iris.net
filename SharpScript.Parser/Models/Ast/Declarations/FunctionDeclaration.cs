using SharpScript.Parser.Models.Ast.Expressions;

namespace SharpScript.Parser.Models.Ast.Declarations;

public class FunctionDeclaration : Node
{
    public List<VariableExpression> Arguments { get; set; }
    public ScopedNode ScopedNode { get; }

    public FunctionDeclaration(string name, List<VariableExpression> arguments, ScopedNode scopedNode) : base(name)
    {
        Arguments = arguments;
        ScopedNode = scopedNode;
    }
}