using SharpScript.Evaluator.Models.WrappedTypes;

namespace SharpScript.Evaluator.Models;

public class ScopeEnvironment
{
    public string Name { get; }
    public Guid Id { get; }
    public Dictionary<string, WrappedEntity?> Variables { get; } = new();

    public ScopeEnvironment(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }
}