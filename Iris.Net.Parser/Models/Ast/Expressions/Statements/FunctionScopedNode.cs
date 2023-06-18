namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class FunctionScopedNode : ScopedNode
{
    public FunctionScopedNode() : base(nameof(BreakableScopedNode))
    {
        
    }
}