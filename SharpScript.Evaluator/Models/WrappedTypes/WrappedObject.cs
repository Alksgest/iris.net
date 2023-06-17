namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedObject : WrappedEntity
{
    public Dictionary<string, object> Value => (Dictionary<string, object>)Object;
    
    public WrappedObject(Dictionary<string, object> value, string? name = null) : base(value, name)
    {
    }
    
}