namespace Iris.Net.Evaluator.Attributes.Library;

[AttributeUsage(AttributeTargets.Class)]
public class StandardLibraryModuleAttributeWithName(string name) : AttributeWithName(name);