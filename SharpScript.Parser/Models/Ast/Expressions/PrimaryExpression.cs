using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public abstract class PrimaryExpression : NodeExpression
{
    public string Value { get; }
    public TokenType Type { get; }
    
    protected PrimaryExpression(string name, string value, TokenType type) : base(name)
    {
        Value = value;
        Type = type;
    }
}