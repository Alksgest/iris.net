namespace SharpScript.Parser.Models.Ast.Expressions.EmbeddedTypes;

public class DictionaryExpression : PrimaryExpression<List<KeyValuePair<string, NodeExpression>>>
{
    public DictionaryExpression(List<KeyValuePair<string, NodeExpression>> value) : base(nameof(ArrayExpression), value)
    {
    }
}