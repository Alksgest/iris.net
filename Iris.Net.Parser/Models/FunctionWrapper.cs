using Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;
using Iris.Net.Parser.Models.Ast.Expressions.Statements;

namespace Iris.Net.Parser.Models;

[Serializable]
public class FunctionWrapper(ScopedNode body, List<VariableExpression> arguments)
{
    public ScopedNode Body { get; } = body;
    public List<VariableExpression> Arguments { get; set; } = arguments;
}