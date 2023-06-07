using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class FunctionCallExpression : PrimaryExpression
{
    public FunctionCall FunctionCall { get; }

    public FunctionCallExpression(
        string value,
        FunctionCall functionCall) : base(nameof(FunctionCallExpression), value, TokenType.Identifier)
    {
        FunctionCall = functionCall;
    }
}