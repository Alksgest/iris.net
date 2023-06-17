using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions.EmdebedTypes;

/// <summary>
/// Represents node expression which contains some value of T
/// </summary>
/// <typeparam name="T">Type of the containing value</typeparam>
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