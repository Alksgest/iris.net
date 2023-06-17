namespace SharpScript.Parser.Models.Ast.Expressions.Functions;

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