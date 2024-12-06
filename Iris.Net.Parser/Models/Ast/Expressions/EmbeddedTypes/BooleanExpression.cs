namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class BooleanExpression(bool value) : PrimaryExpression<bool>(nameof(BooleanExpression), value);