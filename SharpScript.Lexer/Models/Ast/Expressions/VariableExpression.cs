namespace SharpScript.Lexer.Models.Ast.Expressions;

public class VariableExpression : NodeExpression
{
    public VariableExpression(string value) : base(nameof(VariableExpression), value, TokenType.Identifier)
    {
    }
}