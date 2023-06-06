using SharpScript.Lexer.Models.Ast;
using SharpScript.Lexer.Models.Ast.Assignments;
using SharpScript.Lexer.Models.Ast.Declarations;
using SharpScript.Lexer.Models.Ast.Expressions;

namespace SharpScript.Lexer;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _currentTokenIndex = 0;

    private readonly List<Node> _statements;

    public Parser(List<Token> tokens)
    {
        _statements = new List<Node>();
        _tokens = tokens;
    }

    public RootNode ParseTokens()
    {
        if (_statements.Count > 0)
        {
            return new RootNode(_statements);
        }

        while (_currentTokenIndex < _tokens.Count)
        {
            _statements.Add(ParseStatement());
        }

        return new RootNode(_statements);
    }

    private Node ParseStatement()
    {
        if (Match(TokenType.Keyword, "const"))
        {
            ++_currentTokenIndex; // skip const key word
            var node = ParseConstVariableDeclaration();
            _ = Expect(TokenType.Punctuation, ";");
            return node;
        }

        if (Match(TokenType.Keyword, "let"))
        {
            ++_currentTokenIndex; // skip let key word
            var node = ParseLetVariableDeclaration();
            _ = Expect(TokenType.Punctuation, ";");
            return node;
        }

        if (Match(TokenType.Identifier))
        {
            var node = ParseIdentifierExpression();
            _ = Expect(TokenType.Punctuation, ";");
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

        if (Match(TokenType.Punctuation, "("))
        {
            return ParseFunctionCall(nameToken.Value);
        }

        throw new Exception("Invalid identifier expression");
    }

    private FunctionCall ParseFunctionCall(string value)
    {
        _ = Expect(TokenType.Punctuation, "(");
        
        var nodes = new List<NodeExpression>();
        while (!Match(TokenType.Punctuation, ")"))
        {
            var expression = ParseExpression();
            if (Match(TokenType.Punctuation, ","))
            {
                _ = Expect(TokenType.Punctuation, ",");
            }
            nodes.Add(expression);
        }
        
        _ = Expect(TokenType.Punctuation, ")");
        
        return new FunctionCall(value, nodes);
    }

    private VariableAssignment ParseVariableAssignment(Token nameToken)
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

        if (Match(TokenType.Operator, "="))
        {
            _ = Expect(TokenType.Operator, "=");
            value = ParseExpression();
        }

        return new VariableDeclaration(nameToken.Value, value);
    }

    private NodeExpression ParseExpression()
    {
        // For now, we'll just handle numbers
        if (Match(TokenType.NumberValue))
        {
            return new NumberExpression(Expect(TokenType.NumberValue).Value);
        }

        if (Match(TokenType.Identifier))
        {
            // understand, that this is a call
            if (!MatchNext(TokenType.Punctuation, "("))
            {
                return new VariableExpression(Expect(TokenType.Identifier).Value);
            }

            var token = Expect(TokenType.Identifier).Value;
            var functionCall = ParseFunctionCall(token);
                
            return new FunctionCallExpression(token, functionCall);
        }

        throw new Exception("Expected a number");
    }

    private bool Match(TokenType type, string? value = null)
    {
        if (_currentTokenIndex >= _tokens.Count) return false;

        var token = _tokens[_currentTokenIndex];
        return token.Type == type && (value == null || token.Value == value);
    }
    
    private bool MatchNext(TokenType type, string? value = null)
    {
        if (_currentTokenIndex >= _tokens.Count) return false;

        var token = _tokens[_currentTokenIndex + 1];
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