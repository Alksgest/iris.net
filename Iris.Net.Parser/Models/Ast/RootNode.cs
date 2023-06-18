namespace Iris.Net.Parser.Models.Ast;

[Serializable]
public class RootNode : Node
{
    public List<Node> Statements { get; }

    public RootNode(List<Node> statements) : base("root")
    {
        Statements = statements;
    }
}