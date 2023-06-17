namespace SharpScript.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class ArrayExpression : PrimaryExpression<List<NodeExpression>>
{
    public ArrayExpression(List<NodeExpression> value) : base(nameof(ArrayExpression), value)
    {
    }
}