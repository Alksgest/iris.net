namespace Iris.Net.Parser.Models.Ast;

[Serializable]
public class RootNode(List<Node> statements) : Node("root")
{
    public List<Node> Statements { get; } = statements;
}