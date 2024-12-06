namespace Iris.Net.Evaluator.Attributes.Library;

[AttributeUsage(AttributeTargets.Method)]
public class StandardLibraryMethodAttributeWithName(string name) : AttributeWithName(name);