namespace Iris.Net.Evaluator.Attributes.Library;

[AttributeUsage(AttributeTargets.Method)]
public class StandardLibraryMethodAttributeWithName : AttributeWithName
{
    public StandardLibraryMethodAttributeWithName(string name) : base(name)
    {
    }
}