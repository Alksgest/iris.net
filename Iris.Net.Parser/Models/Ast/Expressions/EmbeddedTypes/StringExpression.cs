namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class StringExpression(string value) : PrimaryExpression<string>(nameof(NumberExpression), value);