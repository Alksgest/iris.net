namespace SharpScript.Evaluator.Models;

public class ObjectInScope : EmbeddedEntityInScope
{
    public Dictionary<string, object> Value => (Dictionary<string, object>)Object;
    
    public ObjectInScope(Dictionary<string, object> value, string? name = null) : base(value, name)
    {
    }
    
}