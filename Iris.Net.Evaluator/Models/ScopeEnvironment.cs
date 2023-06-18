using Iris.Net.Evaluator.Models.WrappedTypes;

namespace Iris.Net.Evaluator.Models;

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

    public void Clear()
    {
        Variables.Clear();
    }
}