using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class PropertyIdentifierExpression : PrimaryExpression<string>  
{
    public PropertyIdentifierExpression(string value) : base(nameof(PropertyIdentifierExpression), value, TokenType.Identifier)
    {
    }
}