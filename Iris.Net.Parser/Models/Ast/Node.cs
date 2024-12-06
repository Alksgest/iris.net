namespace Iris.Net.Parser.Models.Ast;

[Serializable]
public abstract class Node(string name)
{
    /// <summary>
    /// Name of the Node
    /// It can be a variable name or name of the class
    /// </summary>
    public string Name { get; } = name;
}