using SharpScript.Lexer.Models;
using SharpScript.Parser.Models.Ast.Expressions.EmdebedTypes;

namespace SharpScript.Parser.Models.Ast.Expressions.Statements;

public class VariableExpression : PrimaryExpression<string>
{
    public VariableExpression(string value) : base(nameof(VariableExpression), value, TokenType.Identifier)
    {
    }
}