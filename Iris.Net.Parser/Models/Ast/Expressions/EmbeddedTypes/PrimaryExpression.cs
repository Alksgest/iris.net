namespace Iris.Net.Parser.Models.Ast.Expressions.EmbeddedTypes;


/// <summary>
/// Represents node expression which contains some value of T
/// </summary>
/// <typeparam name="T">Type of the containing value</typeparam>
[Serializable]
public abstract class PrimaryExpression<T> : NodeExpression
{
    public T Value { get; }
    
    protected PrimaryExpression(string name, T value) : base(name)
    {
        Value = value;
    }
}