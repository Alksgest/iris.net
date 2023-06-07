using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class StringExpression : PrimaryExpression
{
    public StringExpression(string value) : base(nameof(NumberExpression), value, TokenType.StringValue)
    {
    }
}