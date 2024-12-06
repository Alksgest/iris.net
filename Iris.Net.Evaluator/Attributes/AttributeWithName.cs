namespace Iris.Net.Evaluator.Attributes;

public abstract class AttributeWithName(string name) : Attribute
{
    public string Name { get; set; } = name;
}