namespace SharpScript.Parser.Models.Ast;

public class ScopedNode : Node
{
    public List<Node> Statements { get; } = new();

    public ScopedNode() : base(nameof(ScopedNode))
    {
    }
}