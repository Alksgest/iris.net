namespace Iris.Net.Parser.Models.Ast.Expressions.Statements;

[Serializable]
public class ScopedNode : Node
{
    public List<Node> Statements { get; } = [];

    public ScopedNode() : base(nameof(ScopedNode))
    {
    }

    protected ScopedNode(string name): base(name)
    {
        
    }
}