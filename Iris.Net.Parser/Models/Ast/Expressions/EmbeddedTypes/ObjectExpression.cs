namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

/// <summary>
/// Is not in use
/// </summary>
[Serializable]
public class ObjectExpression(object value) : PrimaryExpression<object>(nameof(ObjectExpression), value);