namespace SharpScript.Evaluator.Models;

public class DelegateInScope : ObjectInScope
{
    public Delegate Value => (Delegate)Object;

    public DelegateInScope(Delegate value, string name) : base(value, name)
    {
    }
}