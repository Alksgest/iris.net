namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class FunctionCallExpression : NodeExpression
{
    /// <summary>
    /// Function arguments
    /// </summary>
    public List<NodeExpression>? Values { get; }

    public FunctionCallExpression(string name, List<NodeExpression>? values = null) : base(name)
    {
        Values = values;
    }
}