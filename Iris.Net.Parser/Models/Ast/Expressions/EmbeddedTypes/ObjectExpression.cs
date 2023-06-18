namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

/// <summary>
/// Is not in use
/// </summary>
[Serializable]
public class ObjectExpression : PrimaryExpression<object>
{
    public ObjectExpression(object value) : base(nameof(ObjectExpression), value)
    {
    }
}