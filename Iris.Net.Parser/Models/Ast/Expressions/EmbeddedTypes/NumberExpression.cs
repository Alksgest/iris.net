namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class NumberExpression(decimal value) : PrimaryExpression<decimal>(nameof(NumberExpression), value);