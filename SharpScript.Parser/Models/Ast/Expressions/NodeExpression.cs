namespace SharpScript.Parser.Models.Ast.Expressions;

/// <summary>
/// Node expression is a class which represents some kind of expression
/// </summary>
public abstract class NodeExpression : Node
{
    protected NodeExpression(string name) : base(name)
    {
        
    }
}