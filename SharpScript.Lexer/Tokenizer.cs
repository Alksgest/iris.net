using System.Text;
using SharpScript.Lexer.Models;

namespace SharpScript.Lexer;

// TODO: handle math operators +- | add more operators | add unary operators
// TODO: add if else operators expression +- | add support for if else if
// TODO: add true, false, null literals
// TODO: make value generic instead of object
// TODO: add while loop
// TODO: extend evaluations metadata with errors and warnings
// TODO: handle function creation
// TODO: handle scope creation +
// TODO: handle immutability (let mut)
// TODO: add data structures as array, dict, object, class
public class Tokenizer
{
    private List<Token> _tokens = new();

    // TODO: add support for binary operators
    private readonly List<string> _operators = new()
        { "=", "+", "-", "*", "/", "!", ">", "<", ">=", "<=", "==", "!=", "&&", "||", "&", "|" };

    private readonly List<string> _punctuations = new() { ";", "(", ")", ",", "{", "}" };

    private readonly List<string>
        _keyWords = new() { "const", "let", "if", "else", "true", "false", "while", "null" }; // remove const, add mut

    private readonly List<char> _emptySymbols = new() { ' ', '\n', '\t' };
    private readonly List<char> _stringLiteralIdentifiers = new() { '\"' };

    private TokenizerState _tokenizerState = TokenizerState.Start;

    // const a = 5 
    public List<Token> Process(string input)
    {
        var tokenBuilder = new StringBuilder();
        _tokens = new List<Token>();

        foreach (var c in input)
        {
            if (c == ';')
            {
                if (tokenBuilder.Length > 0)
                {
                    _tokens.Add(ParseToken(tokenBuilder.ToString()));
                }

                tokenBuilder.Clear();
                tokenBuilder.Append(c);
                _tokenizerState = TokenizerState.Punctuation;

                continue;
            }

            switch (_tokenizerState)
            {
                case TokenizerState.Start:
                    HandleStart(c, tokenBuilder);
                    break;
                case TokenizerState.Word:
                    HandleWord(c, tokenBuilder);
                    break;
                case TokenizerState.Number:
                    HandleNumber(c, tokenBuilder);
                    break;
                case TokenizerState.Operator:
                    HandleOperator(c, tokenBuilder);
                    break;
                case TokenizerState.Punctuation:
                    HandlePunctuation(c, tokenBuilder);
                    break;
                case TokenizerState.String:
                    HandleStringLiteral(c, tokenBuilder);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (tokenBuilder.Length == 0)
        {
            return _tokens;
        }

        _tokens.Add(ParseToken(tokenBuilder.ToString()));
        tokenBuilder.Clear();

        return _tokens;
    }

    private void HandleStart(char c, StringBuilder tokenBuilder)
    {
        if (char.IsAsciiLetter(c))
        {
            _tokenizerState = TokenizerState.Word;
            tokenBuilder.Append(c);
        }
        else if (char.IsNumber(c))
        {
            _tokenizerState = TokenizerState.Number;
            tokenBuilder.Append(c);
        }
        else if (_operators.Contains($"{c}"))
        {
            _tokenizerState = TokenizerState.Operator;
            tokenBuilder.Append(c);
        }
        else if (_punctuations.Contains($"{c}"))
        {
            _tokenizerState = TokenizerState.Punctuation;
            tokenBuilder.Append(c);
            FinalizeToken(tokenBuilder);
        }
        else if (_stringLiteralIdentifiers.Contains(c))
        {
            _tokenizerState = TokenizerState.String;
            tokenBuilder.Append(c);
        }
        else if (_emptySymbols.Contains(c))
        {
        }
    }

    private void HandleStringLiteral(char c, StringBuilder tokenBuilder)
    {
        var startSymbol = tokenBuilder[0];

        if (c == startSymbol)
        {
            tokenBuilder.Append(c);
            FinalizeToken(tokenBuilder);
        }
        else
        {
            tokenBuilder.Append(c);
        }
    }

    private void HandlePunctuation(char c, StringBuilder tokenBuilder)
    {
        if (_punctuations.Contains($"{c}"))
        {
            // tokenBuilder.Append(c);
            FinalizeToken(tokenBuilder);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder);
        }
    }

    private void HandleOperator(char c, StringBuilder tokenBuilder)
    {
        var intermediateToken = tokenBuilder.ToString();
        if (_operators.Contains($"{intermediateToken}{c}"))
        {
            tokenBuilder.Append(c);
        }
        else if (char.IsNumber(c) || _punctuations.Contains($"{c}"))
        {
            FinalizePrevTokenEndProcessCurrent(c, tokenBuilder);
        }
        else if (char.IsAsciiLetter(c))
        {
            FinalizeToken(tokenBuilder);

            _tokenizerState = TokenizerState.Word;
            tokenBuilder.Append(c);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder);
        }
    }

    private void HandleNumber(char c, StringBuilder tokenBuilder)
    {
        if (char.IsNumber(c))
        {
            tokenBuilder.Append(c);
        }
        else if (_punctuations.Contains($"{c}"))
        {
            FinalizePrevTokenEndProcessCurrent(c, tokenBuilder);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder);
        }
        else if (_operators.Contains($"{c}"))
        {
            FinalizePrevTokenEndProcessCurrent(c, tokenBuilder);
        }
    }

    private void HandleWord(char c, StringBuilder tokenBuilder)
    {
        //TODO: It is possible to have word with integer number at the end 
        if (char.IsAsciiLetter(c))
        {
            _tokenizerState = TokenizerState.Word;
            tokenBuilder.Append(c);
        }
        else if (_punctuations.Contains($"{c}"))
        {
            FinalizePrevTokenEndProcessCurrent(c, tokenBuilder);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder);
        }
        else if (_operators.Contains($"{c}"))
        {
            FinalizePrevTokenEndProcessCurrent(c, tokenBuilder);
        }
    }

    private void FinalizePrevTokenEndProcessCurrent(char c, StringBuilder tokenBuilder)
    {
        FinalizeToken(tokenBuilder);

        _tokens.Add(ParseToken($"{c}"));
    }

    private void FinalizeToken(StringBuilder tokenBuilder)
    {
        var token = tokenBuilder.ToString();
        _tokens.Add(ParseToken(token));
        tokenBuilder.Clear();
        _tokenizerState = TokenizerState.Start;
    }

    private Token ParseToken(string token)
    {
        var type = token switch
        {
            _ when decimal.TryParse(token, out _) => TokenType.NumberValue,
            _ when _operators.Contains(token) => TokenType.Operator,
            _ when _punctuations.Contains(token) => TokenType.Punctuation,
            _ when _keyWords.Contains(token) => TokenType.Keyword,
            _ when _stringLiteralIdentifiers.Contains(token[0]) => TokenType.StringValue,
            _ => TokenType.Identifier
        };

        return new Token
        {
            Type = type,
            Value = token
        };
    }
}