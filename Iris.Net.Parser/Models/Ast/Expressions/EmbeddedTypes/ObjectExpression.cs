namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

/// <summary>
/// Is not in use
/// </summary>
public class ObjectExpression : PrimaryExpression<object>
{
    public ObjectExpression(object value) : base(nameof(ObjectExpression), value)
    {
    }
}