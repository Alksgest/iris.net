namespace Iris.Net.Evaluator.Attributes.Library;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StandardLibraryPropertyAttributeWithName(string name) : AttributeWithName(name);