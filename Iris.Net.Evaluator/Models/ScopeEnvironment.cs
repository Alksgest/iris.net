using Iris.Net.Evaluator.Models.WrappedTypes;

namespace Iris.Net.Evaluator.Models;

public class ScopeEnvironment(string name)
{
    public string Name { get; } = name;
    public Guid Id { get; } = Guid.NewGuid();
    public Dictionary<string, WrappedEntity?> Variables { get; } = new();

    public void Clear()
    {
        Variables.Clear();
    }
}