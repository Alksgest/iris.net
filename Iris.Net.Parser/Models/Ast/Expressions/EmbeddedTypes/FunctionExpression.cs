namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class FunctionExpression : PrimaryExpression<FunctionWrapper>
{

    public FunctionExpression(FunctionWrapper function) : base(nameof(FunctionExpression), function)
    {
    }
}