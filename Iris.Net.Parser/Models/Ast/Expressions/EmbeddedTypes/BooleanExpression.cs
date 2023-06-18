namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class BooleanExpression : PrimaryExpression<bool>
{
    public BooleanExpression(bool value) : base(nameof(BooleanExpression), value)
    {
    }
}