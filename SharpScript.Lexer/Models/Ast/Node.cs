namespace SharpScript.Lexer.Models.Ast;

public abstract class Node
{
}

public class VariableDeclaration : Node
{
    public string Name { get; }
    public Node? Value { get; }

    public VariableDeclaration(string name, Node? value)
    {
        Name = name;
        Value = value;
    }
}

public class NumberExpression : Node
{
    public string Value { get; }

    public NumberExpression(string value)
    {
        Value = value;
    }
}

public class Tree
{
    public List<Node> Statements { get; }

    public Tree(List<Node> statements)
    {
        Statements = statements;
    }
}