namespace Iris.Net.Parser.Models.Ast;

public abstract class Node
{
    /// <summary>
    /// Name of the Node
    /// It can be variable name or name of the class
    /// </summary>
    public string Name { get; }

    protected Node(string name)
    {
        Name = name;
    }
}