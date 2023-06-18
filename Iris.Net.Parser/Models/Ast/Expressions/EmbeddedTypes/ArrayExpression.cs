namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class ArrayExpression : PrimaryExpression<List<NodeExpression>>
{
    public ArrayExpression(List<NodeExpression> value) : base(nameof(ArrayExpression), value)
    {
    }
}