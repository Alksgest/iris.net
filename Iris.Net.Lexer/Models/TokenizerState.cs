namespace Iris.Net.Lexer.Models;

public enum TokenizerState
{
    Start = 0,
    Word,
    Number,
    String,
    Operator,
    Punctuation
}