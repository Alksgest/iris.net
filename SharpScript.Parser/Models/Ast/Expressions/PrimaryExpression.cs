using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public abstract class PrimaryExpression<T> : NodeExpression
{
    public T Value { get; }
    public TokenType Type { get; }
    
    protected PrimaryExpression(string name, T value, TokenType type) : base(name)
    {
        Value = value;
        Type = type;
    }
}