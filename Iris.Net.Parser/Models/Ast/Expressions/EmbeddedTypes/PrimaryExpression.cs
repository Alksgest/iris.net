namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;


/// <summary>
/// Represents a node expression which contains some value of T
/// </summary>
/// <typeparam name="T">Type of the containing value</typeparam>
[Serializable]
public abstract class PrimaryExpression<T>(string name, T value) : NodeExpression(name)
{
    public T Value { get; } = value;
}