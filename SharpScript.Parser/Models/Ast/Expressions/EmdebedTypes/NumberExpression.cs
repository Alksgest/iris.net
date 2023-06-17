using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions.EmdebedTypes;

public class NumberExpression : PrimaryExpression<decimal>
{
    public NumberExpression(decimal value) : base(nameof(NumberExpression), value, TokenType.NumberValue)
    {
    }
}