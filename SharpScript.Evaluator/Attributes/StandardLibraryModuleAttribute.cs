namespace SharpScript.Evaluator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class StandardLibraryModuleAttribute : Attribute
{
    public string Name { get; set; }

    public StandardLibraryModuleAttribute(string name)
    {
        Name = name;
    }
}