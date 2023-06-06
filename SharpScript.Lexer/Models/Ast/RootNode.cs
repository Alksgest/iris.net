namespace SharpScript.Lexer.Models.Ast;

public class RootNode : Node
{
    public List<Node> Statements { get; }

    public RootNode(List<Node> statements) : base("root")
    {
        Statements = statements;
    }
}