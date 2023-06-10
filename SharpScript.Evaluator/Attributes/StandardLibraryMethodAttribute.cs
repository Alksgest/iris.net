namespace SharpScript.Evaluator.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class StandardLibraryMethodAttribute : Attribute
{
    public string Name { get; set; }
}