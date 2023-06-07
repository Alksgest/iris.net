namespace SharpScript.Parser.Models.Ast;

public abstract class Node
{
    public string Name { get; }

    protected Node(string name)
    {
        Name = name;
    }
}