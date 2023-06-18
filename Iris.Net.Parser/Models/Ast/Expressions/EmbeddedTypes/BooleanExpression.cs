namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class BooleanExpression : PrimaryExpression<bool>
{
    public BooleanExpression(bool value) : base(nameof(BooleanExpression), value)
    {
    }
}