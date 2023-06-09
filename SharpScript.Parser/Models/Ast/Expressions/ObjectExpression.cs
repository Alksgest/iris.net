using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class ObjectExpression : PrimaryExpression<object>
{
    public ObjectExpression(object value) : base(nameof(ObjectExpression), value, TokenType.Identifier)
    {
    }
}