namespace Iris.Net.Evaluator.Attributes.Library;

[AttributeUsage(AttributeTargets.Class)]
public class StandardLibraryModuleAttributeWithName : AttributeWithName
{
    public StandardLibraryModuleAttributeWithName(string name) : base(name)
    {
    }
}