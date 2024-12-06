namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class FunctionCallExpression(string name, List<NodeExpression>? values = null) : NodeExpression(name)
{
    /// <summary>
    /// Function arguments
    /// </summary>
    public List<NodeExpression>? Values { get; } = values;
}