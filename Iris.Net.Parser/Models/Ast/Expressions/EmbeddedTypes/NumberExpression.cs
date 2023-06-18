namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class NumberExpression : PrimaryExpression<decimal>
{
    public NumberExpression(decimal value) : base(nameof(NumberExpression), value)
    {
    }
}