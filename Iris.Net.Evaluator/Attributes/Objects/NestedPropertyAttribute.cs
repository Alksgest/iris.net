namespace Iris.Net.Evaluator.Attributes.Objects;

[AttributeUsage(AttributeTargets.Property)]
public class NestedPropertyAttribute : AttributeWithName
{
    public NestedPropertyAttribute(string name) : base(name)
    {
    }
}