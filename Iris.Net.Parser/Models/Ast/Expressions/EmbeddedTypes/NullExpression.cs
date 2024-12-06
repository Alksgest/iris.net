namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class NullExpression() : PrimaryExpression<object?>(nameof(NullExpression), null);