namespace SharpScript.Evaluator.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StandardLibraryPropertyAttribute : Attribute
{
    public string Name { get; set; }

    public StandardLibraryPropertyAttribute(string name)
    {
        Name = name;
    }

}