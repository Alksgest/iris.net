using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class VariableExpression : PrimaryExpression
{
    public VariableExpression(string value) : base(nameof(VariableExpression), value, TokenType.Identifier)
    {
    }
}