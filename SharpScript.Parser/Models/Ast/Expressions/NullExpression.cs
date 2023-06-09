using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class NullExpression : PrimaryExpression<object?>
{
    public NullExpression() : base(nameof(BooleanExpression), null, TokenType.NumberValue)
    {
    }
}