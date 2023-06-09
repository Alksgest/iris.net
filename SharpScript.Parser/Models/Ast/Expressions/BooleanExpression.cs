using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class BooleanExpression : PrimaryExpression
{
    public BooleanExpression(string value) : base(nameof(BooleanExpression), value, TokenType.NumberValue)
    {
    }
}