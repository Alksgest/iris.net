using SharpScript.Parser.Models.Ast.Expressions;

namespace SharpScript.Parser.Models.Ast;

public class FunctionCall : Node
{
    /// <summary>
    /// Function arguments
    /// </summary>
    public List<NodeExpression>? Values { get; }

    public FunctionCall(string name, List<NodeExpression>? values = null) : base(name)
    {
        Values = values;
    }
}