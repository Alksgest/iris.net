namespace SharpScript.Lexer.Models.Ast.Expressions;

public class FunctionCallExpression : NodeExpression
{
    public FunctionCall FunctionCall { get; }

    public FunctionCallExpression(
        string value,
        FunctionCall functionCall) : base(nameof(FunctionCallExpression), value, TokenType.Identifier)
    {
        FunctionCall = functionCall;
    }
}