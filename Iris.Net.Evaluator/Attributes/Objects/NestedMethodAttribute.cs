namespace Iris.Net.Evaluator.Attributes.Objects;

[AttributeUsage(AttributeTargets.Method)]
public class NestedMethodAttribute : AttributeWithName
{
    public NestedMethodAttribute(string name) : base(name)
    {
    }
}