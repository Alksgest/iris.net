namespace SharpScript.Lexer.Models.Ast.Expressions;

public class StringExpression : NodeExpression
{
    public StringExpression(string value) : base(nameof(NumberExpression), value, TokenType.StringValue)
    {
    }
}