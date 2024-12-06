using Iris.Net.Parser.Models.Ast.Assignments;
using Iris.Net.Parser.Models.Ast.Declarations;

namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

public class ForExpression(
    VariableDeclaration initializer,
    NodeExpression condition,
    VariableAssignment increment,
    BreakableScopedNode? body = null)
    : NodeExpression(nameof(ForExpression))
{
    public VariableDeclaration Initializer { get; } = initializer;
    public NodeExpression Condition { get; } = condition;
    public VariableAssignment Increment { get; } = increment;
    public BreakableScopedNode? Body { get; } = body;
}