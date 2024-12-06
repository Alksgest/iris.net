using Iris.Net.Parser.Models.Ast.Assignments;
using Iris.Net.Parser.Models.Ast.Declarations;

namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

public class ForExpression : NodeExpression
{
    public VariableDeclaration Initializer { get; }
    public NodeExpression Condition { get; }
    public VariableAssignment Increment { get; }
    public BreakableScopedNode? Body { get; }

    public ForExpression(
        VariableDeclaration initializer,
        NodeExpression condition,
        VariableAssignment increment,
        BreakableScopedNode? body = null) : base(nameof(ForExpression))
    {
        Initializer = initializer;
        Condition = condition;
        Increment = increment;
        Body = body;
    }
}