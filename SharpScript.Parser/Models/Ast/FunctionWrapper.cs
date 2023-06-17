using SharpScript.Parser.Models.Ast.Expressions;

namespace SharpScript.Parser.Models.Ast;

public class FunctionWrapper
{
    public ScopedNode Body { get; }
    public List<VariableExpression> Arguments { get; set; }

    public FunctionWrapper(ScopedNode body, List<VariableExpression> arguments)
    {
        Body = body;
        Arguments = arguments;
    }
}