namespace SharpScript.Lexer.Models;

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; } = "";

    public override string ToString()
    {
        return $"{Type}: {Value}";
    }
}