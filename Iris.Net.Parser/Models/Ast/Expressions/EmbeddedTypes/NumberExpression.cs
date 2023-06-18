namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class NumberExpression : PrimaryExpression<decimal>
{
    public NumberExpression(decimal value) : base(nameof(NumberExpression), value)
    {
    }
}