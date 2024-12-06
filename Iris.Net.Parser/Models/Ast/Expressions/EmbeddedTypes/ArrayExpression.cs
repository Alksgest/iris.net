namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class ArrayExpression(List<NodeExpression> value)
    : PrimaryExpression<List<NodeExpression>>(nameof(ArrayExpression), value);