namespace Iris.Net.Parser.Models.Ast.Expressions;

/// <summary>
/// Node expression is a class which represents some kind of expression
/// </summary>
[Serializable]
public abstract class NodeExpression(string name) : Node(name);