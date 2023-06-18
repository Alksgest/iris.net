namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class StringExpression : PrimaryExpression<string>
{
    public StringExpression(string value) : base(nameof(NumberExpression), value)
    {
    }
}