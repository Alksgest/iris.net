namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class VariableExpression(string value) : PrimaryExpression<string>(nameof(VariableExpression), value);