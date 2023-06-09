using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class StringExpression : PrimaryExpression<string>
{
    public StringExpression(string value) : base(nameof(NumberExpression), value, TokenType.StringValue)
    {
    }
}