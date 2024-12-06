namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;

[Serializable]
public class FunctionExpression(FunctionWrapper function)
    : PrimaryExpression<FunctionWrapper>(nameof(FunctionExpression), function);