namespace Iris.Net.Evaluator.Attributes.Objects;

[AttributeUsage(AttributeTargets.Method)]
public class NestedMethodAttribute(string name) : AttributeWithName(name);