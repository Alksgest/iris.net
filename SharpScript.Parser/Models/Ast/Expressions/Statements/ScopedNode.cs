namespace SharpScript.Parser.Models.Ast.Expressions.Statements;

public class ScopedNode : Node
{
    public List<Node> Statements { get; } = new();

    public ScopedNode() : base(nameof(ScopedNode))
    {
    }

    protected ScopedNode(string name): base(name)
    {
        
    }
}