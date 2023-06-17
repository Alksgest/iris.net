namespace SharpScript.Evaluator.Attributes.Library;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StandardLibraryPropertyAttributeWithName : AttributeWithName
{
    public StandardLibraryPropertyAttributeWithName(string name) : base(name)
    {
    }
}