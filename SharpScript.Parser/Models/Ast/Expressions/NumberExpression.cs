using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class NumberExpression : PrimaryExpression
{
    public NumberExpression(string value) : base(nameof(NumberExpression), value, TokenType.NumberValue)
    {
    }
}