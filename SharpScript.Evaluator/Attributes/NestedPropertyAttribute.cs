namespace SharpScript.Evaluator.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NestedPropertyAttribute : Attribute
{
    public string Name { get; set; }
}