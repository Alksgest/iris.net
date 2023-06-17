using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions.EmdebedTypes;

/// <summary>
/// Is not in use
/// </summary>
public class ObjectExpression : PrimaryExpression<object>
{
    public ObjectExpression(object value) : base(nameof(ObjectExpression), value, TokenType.Identifier)
    {
    }
}