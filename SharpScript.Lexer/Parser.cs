using SharpScript.Lexer.Models.Ast;

namespace SharpScript.Lexer;

public class Parser
{
    private List<Token> _tokens;
    private int _currentTokenIndex = 0;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Tree ParseProgram()
    {
        var statements = new List<Node>();
        while (_currentTokenIndex < _tokens.Count)
        {
            statements.Add(ParseStatement());
        }

        return new Tree(statements);
    }

    private Node ParseStatement()
    {
        if (Match(TokenType.Keyword, "const"))
        {
            ++_currentTokenIndex;
            return ParseConstVariableDeclaration();
        }

        if (Match(TokenType.Keyword, "let"))
        {
            ++_currentTokenIndex;
            return ParseLetVariableDeclaration();
        }

        throw new Exception("Expected a statement");
    }

    private VariableDeclaration ParseConstVariableDeclaration()
    {
        var nameToken = Expect(TokenType.Identifier);
        _ = Expect(TokenType.Operator, "=");
        var value = ParseExpression();
        Expect(TokenType.Punctuation, ";");
        return new VariableDeclaration(nameToken.Value, value);
    }

    private VariableDeclaration ParseLetVariableDeclaration()
    {
        var nameToken = Expect(TokenType.Identifier);
        Node value = null;
        try
        {
            _ = Expect(TokenType.Operator, "=");
            value = ParseExpression();
        }
        catch
        {
            // ignored
        }

        Expect(TokenType.Punctuation, ";");
        return new VariableDeclaration(nameToken.Value, value);
    }

    private Node ParseExpression()
    {
        // For now, we'll just handle numbers
        if (Match(TokenType.NumberValue))
        {
            return new NumberExpression(Expect(TokenType.NumberValue).Value);
        }

        throw new Exception("Expected a number");
    }

    private bool Match(TokenType type, string? value = null)
    {
        if (_currentTokenIndex >= _tokens.Count) return false;

        var token = _tokens[_currentTokenIndex];
        return token.Type == type && (value == null || token.Value == value);
    }

    private Token Expect(TokenType type, string? value = null)
    {
        if (Match(type, value))
        {
            return _tokens[_currentTokenIndex++];
        }

        throw new Exception($"Expected token {type} but got {_tokens[_currentTokenIndex].Type}");
    }
}