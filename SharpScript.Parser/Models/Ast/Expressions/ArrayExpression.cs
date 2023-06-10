using SharpScript.Lexer.Models;

namespace SharpScript.Parser.Models.Ast.Expressions;

public class ArrayExpression : PrimaryExpression<List<NodeExpression>>
{
    public ArrayExpression(List<NodeExpression> value) : base(nameof(ArrayExpression), value, TokenType.Identifier)
    {
    }
}