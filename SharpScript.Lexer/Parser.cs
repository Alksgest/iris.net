using SharpScript.Lexer.Models.Ast;

namespace SharpScript.Lexer;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _currentTokenIndex = 0;

    private List<Node> _statements;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Tree ParseTokens()
    {
        _statements = new List<Node>();
        while (_currentTokenIndex < _tokens.Count)
        {
            _statements.Add(ParseStatement());
        }

        return new Tree(_statements);
    }

    private Node ParseStatement()
    {
        if (Match(TokenType.Keyword, "const"))
        {
            ++_currentTokenIndex; // skip const key word
            var node =  ParseConstVariableDeclaration();
            Expect(TokenType.Punctuation, ";");
            return node;
        }

        if (Match(TokenType.Keyword, "let"))
        {
            ++_currentTokenIndex; // skip let key word
            var node =  ParseLetVariableDeclaration();
            Expect(TokenType.Punctuation, ";");
            return node;
        }

        if (Match(TokenType.Identifier))
        {
            var node =  ParseIdentifierExpression();
            Expect(TokenType.Punctuation, ";");
            return node;
        }

        throw new Exception("Expected a statement");
    }

    private Node ParseIdentifierExpression()
    {
        // assign value if left part if variable name
        var nameToken = Expect(TokenType.Identifier);
        if (Match(TokenType.Operator, "="))
        {
            return ParseVariableAssignment(nameToken);
        }
        
        // handle function call

        throw new Exception("Invalid identifier expression");
    }

    private Node ParseVariableAssignment(Token nameToken)
    {
        _ = Expect(TokenType.Operator, "=");
        var value = ParseExpression();
        return new VariableAssignment(nameToken.Value, value);
    }

    private VariableDeclaration ParseConstVariableDeclaration()
    {
        var nameToken = Expect(TokenType.Identifier);
        _ = Expect(TokenType.Operator, "=");
        var value = ParseExpression();
        return new VariableDeclaration(nameToken.Value, value);
    }

    private VariableDeclaration ParseLetVariableDeclaration()
    {
        var nameToken = Expect(TokenType.Identifier);
        Node? value = null;
        try
        {
            _ = Expect(TokenType.Operator, "=");
            value = ParseExpression();
        }
        catch
        {
            // ignored
        }

        return new VariableDeclaration(nameToken.Value, value);
    }

    private Node? ParseExpression()
    {
        // For now, we'll just handle numbers
        if (Match(TokenType.NumberValue))
        {
            return new NumberExpression(Expect(TokenType.NumberValue).Value);
        }

        if (Match(TokenType.Identifier))
        {
            return new VariableExpression(Expect(TokenType.Identifier).Value);
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