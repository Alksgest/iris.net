namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedDictionary : WrappedEntity
{
    public Dictionary<string, object> Value => (Dictionary<string, object>)Object;
    
    public WrappedDictionary(Dictionary<string, object> value, string? name = null) : base(value, name)
    {
    }
}