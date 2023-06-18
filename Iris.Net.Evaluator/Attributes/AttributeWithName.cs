namespace Iris.Net.Evaluator.Attributes;

public abstract class AttributeWithName : Attribute
{
    public string Name { get; set; }

    protected AttributeWithName(string name)
    {
        Name = name;
    }
}