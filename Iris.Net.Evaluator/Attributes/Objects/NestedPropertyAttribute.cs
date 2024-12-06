namespace Iris.Net.Evaluator.Attributes.Objects;

[AttributeUsage(AttributeTargets.Property)]
public class NestedPropertyAttribute(string name) : AttributeWithName(name);