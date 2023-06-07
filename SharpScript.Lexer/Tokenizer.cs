using System.Text;

namespace SharpScript.Lexer;

public enum TokenType
{
    Identifier = 0,
    NumberValue,
    StringValue,
    Operator,
    Keyword,
    Punctuation
}

public enum TokenizerState
{
    Start = 0,
    Word,
    Number,
    String,
    Operator,
    Punctuation
}

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; } = "";
}

// TODO: handle math operators
// TODO: add if else operators expression
// TODO: add while loop
// TODO: handle function creation
// TODO: handle scope creation
// TODO: handle immutability (let mut)
// TODO: extend evaluations metadata
public class Tokenizer
{
    private readonly List<string> _operators = new() { "=" };
    private readonly List<string> _punctuations = new() { ";", "(", ")", "," };
    private readonly List<string> _keyWords = new() { "const", "let" }; // remove const, add mut
    private readonly List<char> _emptySymbols = new() { ' ', '\n', '\t' };
    private readonly List<char> _stringLiteralIdentifiers = new() { '\"' };

    private TokenizerState _tokenizerState = TokenizerState.Start;

    // const a = 5 
    public List<Token> Process(string input)
    {
        var tokenBuilder = new StringBuilder();
        var tokens = new List<Token>();

        foreach (var c in input)
        {
            if (c == ';')
            {
                if (tokenBuilder.Length > 0)
                {
                    tokens.Add(ParseToken(tokenBuilder.ToString()));
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
                    HandleWord(c, tokenBuilder, tokens);
                    break;
                case TokenizerState.Number:
                    HandleNumber(c, tokenBuilder, tokens);
                    break;
                case TokenizerState.Operator:
                    HandleOperator(c, tokenBuilder, tokens);
                    break;
                case TokenizerState.Punctuation:
                    HandlePunctuation(c, tokenBuilder, tokens);
                    break;
                case TokenizerState.String:
                    HandleStringLiteral(c, tokenBuilder, tokens);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (tokenBuilder.Length == 0)
        {
            return tokens;
        }

        tokens.Add(ParseToken(tokenBuilder.ToString()));
        tokenBuilder.Clear();

        return tokens;
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
        } else if (_stringLiteralIdentifiers.Contains(c))
        {
            _tokenizerState = TokenizerState.String;
            tokenBuilder.Append(c);
        }
        else if (_emptySymbols.Contains(c))
        {
        }
    }

    private void HandleStringLiteral(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        var startSymbol = tokenBuilder[0];

        if (c == startSymbol)
        {
            tokenBuilder.Append(c);
            FinalizeToken(tokenBuilder, tokens);
        }
        else
        {
            tokenBuilder.Append(c);
        }
    }

    private void HandlePunctuation(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        if (_punctuations.Contains($"{c}"))
        {
            tokenBuilder.Append(c);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder, tokens);
        }
    }

    private void HandleOperator(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        var intermediateToken = tokenBuilder.ToString();
        if (_operators.Contains($"{intermediateToken}{c}"))
        {
            tokenBuilder.Append(c);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder, tokens);
        }
    }

    private void HandleNumber(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        if (char.IsNumber(c))
        {
            tokenBuilder.Append(c);
        }
        else if (_punctuations.Contains($"{c}"))
        {
            HandlePunctuationStart(c, tokenBuilder, tokens);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder, tokens);
        }
    }

    private void HandleWord(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        //TODO: It is possible to have word with integer number at the end 
        if (char.IsAsciiLetter(c))
        {
            _tokenizerState = TokenizerState.Word;
            tokenBuilder.Append(c);
        }
        else if (_punctuations.Contains($"{c}"))
        {
            HandlePunctuationStart(c, tokenBuilder, tokens);
        }
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder, tokens);
        }
    }

    private void HandlePunctuationStart(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        var token = tokenBuilder.ToString();
        tokens.Add(ParseToken(token));
        tokenBuilder.Clear();
        _tokenizerState = TokenizerState.Start;
        tokens.Add(ParseToken($"{c}"));
    }

    private void FinalizeToken(StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        _tokenizerState = TokenizerState.Start;
        var token = tokenBuilder.ToString();
        tokens.Add(ParseToken(token));

        tokenBuilder.Clear();
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