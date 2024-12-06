namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedDictionary(Dictionary<string, object> value, string? name = null) : WrappedEntity(value, name)
{
    public Dictionary<string, object> Value => (Dictionary<string, object>)Object;
}