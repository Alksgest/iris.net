namespace Iris.Net.Lexer.Models;

public class Token
{
    public TokenType Type { get; init; }
    public string Value { get; init; } = "";

    public override string ToString()
    {
        return $"{Type}: {Value}";
    }
}