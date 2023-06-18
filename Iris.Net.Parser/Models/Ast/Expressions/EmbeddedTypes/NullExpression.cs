namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class NullExpression : PrimaryExpression<object?>
{
    public NullExpression() : base(nameof(NullExpression), null)
    {
    }
}