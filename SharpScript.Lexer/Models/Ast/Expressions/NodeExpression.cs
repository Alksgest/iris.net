namespace SharpScript.Lexer.Models.Ast.Expressions;

public abstract class NodeExpression : Node
{
    public string Value { get; }
    public TokenType Type { get; }

    protected NodeExpression(string name, string value, TokenType type) : base(name)
    {
        Value = value;
        Type = type;
    }
}