namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class DictionaryExpression(List<KeyValuePair<string, NodeExpression>> value)
    : PrimaryExpression<List<KeyValuePair<string, NodeExpression>>>(nameof(ArrayExpression), value);