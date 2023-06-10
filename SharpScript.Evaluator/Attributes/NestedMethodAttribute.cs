namespace SharpScript.Evaluator.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class NestedMethodAttribute : Attribute
{
    public string Name { get; set; }
}