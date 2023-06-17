using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions.EmdebedTypes;

public class NullExpression : PrimaryExpression<object?>
{
    public NullExpression() : base(nameof(NullExpression), null, TokenType.NumberValue)
    {
    }
}