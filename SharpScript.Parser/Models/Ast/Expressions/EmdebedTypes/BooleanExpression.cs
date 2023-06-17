using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions.EmdebedTypes;

public class BooleanExpression : PrimaryExpression<bool>
{
    public BooleanExpression(bool value) : base(nameof(BooleanExpression), value, TokenType.NumberValue)
    {
    }
}