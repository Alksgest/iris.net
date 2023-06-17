namespace SharpScript.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class FunctionExpression : PrimaryExpression<FunctionWrapper>
{

    public FunctionExpression(FunctionWrapper function) : base(nameof(FunctionExpression), function)
    {
    }
}