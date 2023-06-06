// ReSharper disable InconsistentNaming

using System.Text;

namespace SharpScript.Lexer;

//
public static class Keyword
{
    public const string @if = "if";
    public const string @else = "else";
    public const string @for = "for";
    public const string @while = "while";
    public const string @const = "const";
    public const string @mut = "mut";
}

public static class Operators
{
    public const string plus = "+";
    public const string minus = "-";
    public const string divide = "/";
    public const string multiply = "*";
    public const string equal = "=";
}

public static class Punctuation
{
    private const string leftParenthesis = "(";
    private const string rightParenthesis = ")";
    private const string leftCurlyBracket = ")";
    private const string rightCurlyBracket = ")";
}

public enum TokenType
{
    Identifier = 0,
    NumberValue,
    Operator,
    Keyword,
    Punctuation
}

public enum TokenizerState
{
    Start = 0,
    Word,
    Number,
    Operator,
    Punctuation
}

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; } = "";
}

public class Tokenizer
{
    private readonly List<string> _operators = new() { "=" };
    private readonly List<string> _punctuations = new() { ";" };
    private readonly List<string> _keyWords = new() { "const", "let" };
    private readonly List<char> _emptySymbols = new() { ' ', '\n', '\t' };

    private TokenizerState _tokenizerState = TokenizerState.Start;

    // const a = 5 
    public List<Token> Process(string input)
    {
        var tokenBuilder = new StringBuilder();
        var tokens = new List<Token>();

        foreach (var c in input)
        {
            if (_punctuations.Contains($"{c}"))
            {
                tokens.Add(ParseToken(tokenBuilder.ToString()));
                tokenBuilder.Clear();
                tokenBuilder.Append(c);
                _tokenizerState = TokenizerState.Punctuation;
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

    private void HandlePunctuation(char c, StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        // Right now we handle only the end of statement char ';'
        FinalizeToken(tokenBuilder, tokens);
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
        else if (_emptySymbols.Contains(c))
        {
            FinalizeToken(tokenBuilder, tokens);
        }
    }

    private void FinalizeToken(StringBuilder tokenBuilder, ICollection<Token> tokens)
    {
        _tokenizerState = TokenizerState.Start;
        var token = tokenBuilder.ToString();
        tokens.Add(ParseToken(token));

        tokenBuilder.Clear();
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
        }
        else if (_emptySymbols.Contains(c))
        {

        }
    }

    private Token ParseToken(string token)
    {
        var type = token switch
        {
            _ when decimal.TryParse(token, out var _) => TokenType.NumberValue,
            _ when _operators.Contains(token) => TokenType.Operator,
            _ when _punctuations.Contains(token) => TokenType.Punctuation,
            _ when _keyWords.Contains(token) => TokenType.Keyword,
            _ => TokenType.Identifier
        };

        return new Token
        {
            Type = type,
            Value = token
        };
    }
}