namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class VariableExpression : PrimaryExpression<string>
{
    public VariableExpression(string value) : base(nameof(VariableExpression), value)
    {
    }
}