using SharpScript.Parser.Models.Ast.Expressions.EmbeddedTypes;
using SharpScript.Parser.Models.Ast.Expressions.Statements;

namespace SharpScript.Parser.Models;

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