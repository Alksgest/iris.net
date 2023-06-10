namespace SharpScript.Evaluator.Models;

public abstract class ObjectInScope
{
    public object Object { get; }

    protected ObjectInScope(object o)
    {
        Object = o;
    }
}