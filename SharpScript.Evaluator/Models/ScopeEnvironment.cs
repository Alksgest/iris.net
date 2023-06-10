using System.Reflection;

namespace SharpScript.Evaluator.Models;

public abstract class ObjectInScope
{
    public object Object { get; }

    protected ObjectInScope(object o)
    {
        Object = o;
    }
}

public class PrimitiveValueInScope<T> : ObjectInScope where T : notnull
{
    public T Value => (T)Object;

    public PrimitiveValueInScope(T value) : base(value)
    {
    }
}

public class ArrayInScope : ObjectInScope
{
    public List<object> Value => (List<object>)(Object);

    public int Length => Value.Count;

    public ArrayInScope(List<object> value) : base(value)
    {
    }

    public int LengthMethod()
    {
        return Value.Count;
    }
}

public class DelegateInScope : ObjectInScope
{
    public Delegate Value => (Delegate)Object;

    public DelegateInScope(Delegate value) : base(value)
    {
    }
}

public class MethodInScope : ObjectInScope
{
    public MethodInfo Value => (MethodInfo)Object;

    public MethodInScope(MethodInfo value) : base(value)
    {
    }
}

public class ScopeEnvironment
{
    public string Name { get; }
    public Guid Id { get; }
    public Dictionary<string, ObjectInScope?> Variables { get; } = new();

    public ScopeEnvironment(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }
}