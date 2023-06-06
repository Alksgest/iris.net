namespace SharpScript.Lexer.Models.Ast.Expressions;

public class NumberExpression : NodeExpression
{
    public NumberExpression(string value) : base(nameof(NumberExpression), value, TokenType.NumberValue)
    {
    }
}