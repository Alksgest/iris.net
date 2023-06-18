using Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;
using Iris.Net.Parser.Models.Ast.Expressions.Statements;

namespace Iris.Net.Parser.Models;

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