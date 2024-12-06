using Iris.Net.Lexer.Models;
using Iris.Net.Parser.Models;
using Iris.Net.Parser.Models.Ast;
using Iris.Net.Parser.Models.Ast.Assignments;
using Iris.Net.Parser.Models.Ast.Declarations;
using Iris.Net.Parser.Models.Ast.Expressions;
using Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;
using Iris.Net.Parser.Models.Ast.Expressions.Statements;

namespace Iris.Net.Parser;

public class TokensParser
{
    private readonly List<Token> _tokens;
    private int _currentTokenIndex;

    private readonly List<Node> _statements;

    public TokensParser(List<Token> tokens)
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
        if (Match(TokenType.Keyword, ["let", "const"]))
        {
            var node = ParseVariableDeclaration();

            if (!MatchPrev(TokenType.Punctuation, "}"))
            {
                _ = Expect(TokenType.Punctuation, ";");
            }

            return node;
        }

        if (Match(TokenType.Keyword, "if"))
        {
            _ = Expect(TokenType.Keyword, "if");
            var node = ParseConditionalExpression();
            return node;
        }

        if (Match(TokenType.Keyword, "while"))
        {
            _ = Expect(TokenType.Keyword, "while");
            var condition = ParseExpression();
            var body = ParseScopedNode(ScopeType.Breakable) as BreakableScopedNode;

            return new WhileExpression(condition, body!);
        }

        if (Match(TokenType.Keyword, "foreach"))
        {
            var hasParentheses = false;
            _ = Expect(TokenType.Keyword, "foreach");
            if (Match(TokenType.Punctuation, "("))
            {
                hasParentheses = true;
                _ = Expect(TokenType.Punctuation, "(");
            }

            _ = Expect(TokenType.Keyword, ["let", "const"]);
            var token = Expect(TokenType.Identifier);
            var variableExpression = new VariableExpression(token.Value);
            _ = Expect(TokenType.Keyword, "in");
            var iEnumerable = ParseExpression();

            if (hasParentheses)
            {
                _ = Expect(TokenType.Punctuation, ")");
            }

            var body = ParseScopedNode(ScopeType.Breakable) as BreakableScopedNode;

            return new ForeachExpression(variableExpression, iEnumerable, body!);
        }

        if (Match(TokenType.Keyword, "for"))
        {
            var hasParentheses = false;
            _ = Expect(TokenType.Keyword, "for");
            if (Match(TokenType.Punctuation, "("))
            {
                hasParentheses = true;
                _ = Expect(TokenType.Punctuation, "(");
            }
            
            var initializer = ParseVariableDeclaration();
            _ = Expect(TokenType.Punctuation, ";");
            
            var condition = ParseExpression();
            _ = Expect(TokenType.Punctuation, ";");

            var token = Expect(TokenType.Identifier);
            var increment = ParseVariableAssignment(token);

            if (hasParentheses)
            {
                _ = Expect(TokenType.Punctuation, ")");
            }

            if (Match(TokenType.Punctuation, "{"))
            {
                var body = ParseScopedNode(ScopeType.Breakable) as BreakableScopedNode;
                return new ForExpression(initializer, condition, increment, body);
            }

            return new ForExpression(initializer, condition, increment);
        }

        if (Match(TokenType.Keyword, "break"))
        {
            _ = Expect(TokenType.Keyword, "break");
            _ = Expect(TokenType.Punctuation, ";");

            return new BreakExpression();
        }

        if (Match(TokenType.Keyword, "return"))
        {
            _ = Expect(TokenType.Keyword, "return");
            var expression = ParseExpression();
            _ = Expect(TokenType.Punctuation, ";");

            return new ReturnExpression(expression);
        }

        if (Match(TokenType.Keyword, "function"))
        {
            _ = Expect(TokenType.Keyword, "function");

            var name = Expect(TokenType.Identifier).Value;
            var arguments = ParseFunctionArguments();
            var functionBody = ParseScopedNode(ScopeType.Function);

            var function = new FunctionWrapper(functionBody, arguments);

            return new FunctionDeclaration(name, function);
        }

        if (Match(TokenType.Identifier))
        {
            var node = ParseIdentifierExpression();

            if (!MatchPrev(TokenType.Punctuation, "}"))
            {
                _ = Expect(TokenType.Punctuation, ";");
            }

            return node;
        }

        if (Match(TokenType.Punctuation, "{"))
        {
            var node = ParseScopedNode(ScopeType.Ordinary);
            return node;
        }

        if (Match(TokenType.Punctuation, "["))
        {
            var node = ParseStartOfSquareBracket();
            _ = Expect(TokenType.Punctuation, ";");
            return node;
        }

        throw new Exception("Expected a statement");
    }

    private NodeExpression ParseStartOfSquareBracket()
    {
        if (
            _currentTokenIndex == 0 ||
            MatchPrev(TokenType.Punctuation, ";") ||
            MatchPrev(TokenType.Punctuation, "}"))
        {
            return ParseArrayExpression();
        }

        throw new Exception("Unexpected usage of []");
    }

    private List<VariableExpression> ParseFunctionArguments()
    {
        _ = Expect(TokenType.Punctuation, "(");

        var nodes = new List<VariableExpression>();
        while (!Match(TokenType.Punctuation, ")"))
        {
            var expression = new VariableExpression(Expect(TokenType.Identifier).Value);
            if (Match(TokenType.Punctuation, ","))
            {
                _ = Expect(TokenType.Punctuation, ",");
            }

            nodes.Add(expression);
        }

        _ = Expect(TokenType.Punctuation, ")");

        return nodes;
    }

    private ConditionalExpression ParseConditionalExpression()
    {
        var condition = ParseExpression();
        var trueStatement = ParseStatement();

        Node? falseStatement = null;

        if (Match(TokenType.Keyword, "else"))
        {
            _ = Expect(TokenType.Keyword, "else");
            falseStatement = ParseStatement();
        }

        return new ConditionalExpression(condition, trueStatement, falseStatement);
    }

    private ScopedNode ParseScopedNode(ScopeType type)
    {
        _ = Expect(TokenType.Punctuation, "{");

        var scopedNode = type switch
        {
            ScopeType.Ordinary => new ScopedNode(),
            ScopeType.Breakable => new BreakableScopedNode(),
            ScopeType.Function => new FunctionScopedNode(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        while (!Match(TokenType.Punctuation, "}"))
        {
            var node = ParseStatement();
            scopedNode.Statements.Add(node);
        }

        _ = Expect(TokenType.Punctuation, "}");

        return scopedNode;
    }

    private Node ParseIdentifierExpression()
    {
        // assign value if left part is variable name
        var token = Expect(TokenType.Identifier);

        if (Match(TokenType.Operator, "="))
        {
            return ParseVariableAssignment(token);
        }

        if (Match(TokenType.Punctuation, "("))
        {
            return ParseFunctionCall(token.Value);
        }

        _currentTokenIndex--;
        return ParseExpression();
    }

    private FunctionCallExpression ParseFunctionCall(string value)
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

        return new FunctionCallExpression(value, nodes);
    }

    private VariableAssignment ParseVariableAssignment(Token nameToken)
    {
        _ = Expect(TokenType.Operator, "=");

        if (Match(TokenType.Punctuation, "{"))
        {
            var value = ParseDictionaryExpression();
            return new VariableAssignment(nameToken.Value, value);
        }
        else
        {
            var value = ParseExpression();
            return new VariableAssignment(nameToken.Value, value);
        }
    }

    private VariableDeclaration ParseVariableDeclaration()
    {
        var token = Expect(TokenType.Keyword);

        return token.Value switch
        {
            "let" => ParseLetVariableDeclaration(),
            "const" => ParseConstVariableDeclaration(),
            _ => throw new Exception($"Unexpected variable declaration, got {token.Value}")
        };
    }

    private VariableDeclaration ParseConstVariableDeclaration()
    {
        var token = Expect(TokenType.Identifier);
        _ = Expect(TokenType.Operator, "=");
        var value = ParseExpression();
        return new VariableDeclaration(token.Value, value);
    }

    private VariableDeclaration ParseLetVariableDeclaration()
    {
        var token = Expect(TokenType.Identifier);
        Node? value = null;

        if (Match(TokenType.Operator, "="))
        {
            _ = Expect(TokenType.Operator, "=");
            value = ParseExpression();
        }

        return new VariableDeclaration(token.Value, value);
    }

    private NodeExpression ParseExpression()
    {
        return ParseBinaryExpression(0);
    }

    private NodeExpression ParseBinaryExpression(int minPrecedence)
    {
        var expr = ParseUnaryExpression();

        while (true)
        {
            if (Match(TokenType.Punctuation) || MatchPrev(TokenType.Punctuation, "}"))
            {
                break;
            }

            var op = _tokens[_currentTokenIndex];
            if (!Match(TokenType.Operator))
            {
                throw new ArgumentException($"Invalid operator. Got {op}");
            }

            var precedence = GetPrecedence(op);
            if (precedence < minPrecedence)
            {
                break;
            }

            ++_currentTokenIndex;

            var right = ParseBinaryExpression(precedence + 1);
            expr = new BinaryExpression(expr, op.Value, right);
        }

        return expr;
    }

    private NodeExpression ParseUnaryExpression()
    {
        if (Match(TokenType.Punctuation, "("))
        {
            _ = Expect(TokenType.Punctuation, "(");
            var expr = ParseExpression();
            Expect(TokenType.Punctuation, ")");
            return expr;
        }

        if (Match(TokenType.Operator, "-") || Match(TokenType.Operator, "!"))
        {
            var op = Expect(TokenType.Operator).Value;
            var expr = ParseExpression();
            var unaryExpression = new UnaryExpression(expr, op);

            return unaryExpression;
        }

        return ParsePrimary();
    }

    private DictionaryExpression ParseDictionaryExpression()
    {
        _ = Expect(TokenType.Punctuation, "{");
        var pairs = new List<KeyValuePair<string, NodeExpression>>();
        while (!Match(TokenType.Punctuation, "}"))
        {
            var key = Expect(TokenType.Identifier);
            _ = Expect(TokenType.Operator, "=");
            var expression = ParseExpression();

            pairs.Add(new KeyValuePair<string, NodeExpression>(key.Value, expression));

            _ = Expect(TokenType.Punctuation, ",");
        }

        _ = Expect(TokenType.Punctuation, "}");

        return new DictionaryExpression(pairs);
    }

    private ArrayExpression ParseArrayExpression()
    {
        _ = Expect(TokenType.Punctuation, "[");
        var elements = new List<NodeExpression>();
        while (!Match(TokenType.Punctuation, "]"))
        {
            var expr = ParseExpression();
            elements.Add(expr);

            if (Match(TokenType.Punctuation, ","))
            {
                _ = Expect(TokenType.Punctuation, ",");
            }
        }

        _ = Expect(TokenType.Punctuation, "]");
        var arrayExpression = new ArrayExpression(elements);

        return arrayExpression;
    }

    private NodeExpression ParsePrimary()
    {
        if (Match(TokenType.NumberValue))
        {
            var value = decimal.Parse(Expect(TokenType.NumberValue).Value);
            return new NumberExpression(value);
        }

        if (Match(TokenType.Keyword, "true") || Match(TokenType.Keyword, "false"))
        {
            var value = bool.Parse(Expect(TokenType.Keyword).Value);
            return new BooleanExpression(value);
        }

        if (Match(TokenType.StringValue))
        {
            var value = Expect(TokenType.StringValue).Value;
            return new StringExpression(value);
        }

        if (Match(TokenType.Keyword, "null"))
        {
            _ = Expect(TokenType.Keyword, "null");
            return new NullExpression();
        }

        if (Match(TokenType.Keyword, "function"))
        {
            _ = Expect(TokenType.Keyword, "function");

            var arguments = ParseFunctionArguments();
            var body = ParseScopedNode(ScopeType.Function);

            var function = new FunctionWrapper(body, arguments);
            return new FunctionExpression(function);
        }

        if (Match(TokenType.Identifier))
        {
            if (MatchNext(TokenType.Punctuation, "("))
            {
                var token = Expect(TokenType.Identifier).Value;
                var functionCall = ParseFunctionCall(token);

                return functionCall;
            }

            return new VariableExpression(Expect(TokenType.Identifier).Value);
        }

        if (Match(TokenType.Punctuation, "["))
        {
            var node = ParseArrayExpression();
            return node;
        }

        if (Match(TokenType.Punctuation, "{"))
        {
            var node = ParseDictionaryExpression();
            return node;
        }

        throw new Exception($"Unexpected expression, current token is {_tokens[_currentTokenIndex]}");
    }

    private static int GetPrecedence(Token op)
    {
        return op.Value switch
        {
            "." => 10,
            "*" or "/" => 2,
            "+" or "-" => 1,
            _ => 0
        };
    }

    private bool Match(TokenType type, string? value = null)
    {
        if (_currentTokenIndex >= _tokens.Count)
        {
            return false;
        }

        var token = _tokens[_currentTokenIndex];
        return token.Type == type && (value == null || token.Value == value);
    }
    
    private bool Match(TokenType type, IEnumerable<string> values)
    {
        if (_currentTokenIndex >= _tokens.Count)
        {
            return false;
        }

        var token = _tokens[_currentTokenIndex];
        return token.Type == type && values.Contains(token.Value);
    }

    private bool MatchNext(TokenType type, string? value = null)
    {
        if (_currentTokenIndex >= _tokens.Count)
        {
            return false;
        }

        var token = _tokens[_currentTokenIndex + 1];
        return token.Type == type && (value == null || token.Value == value);
    }

    private bool MatchPrev(TokenType type, string? value = null)
    {
        if (_currentTokenIndex > _tokens.Count)
        {
            return false;
        }

        var token = _tokens[_currentTokenIndex - 1];
        return token.Type == type && (value == null || token.Value == value);
    }

    private Token Expect(TokenType type, string? value = null)
    {
        if (Match(type, value))
        {
            return _tokens[_currentTokenIndex++];
        }

        throw new Exception(
            $"Expected token {type} with value {value} but got {_tokens[_currentTokenIndex].Type} with value {_tokens[_currentTokenIndex].Value}");
    }

    private Token Expect(TokenType type, IEnumerable<string> values)
    {
        if (Match(type, values))
        {
            return _tokens[_currentTokenIndex++];
        }

        throw new Exception(
            $"Expected token {type} with one of the value [{string.Concat(values, ',')}] but got {_tokens[_currentTokenIndex].Type} with value {_tokens[_currentTokenIndex].Value}");
    }
}