using System.Reflection;

namespace SharpScript.Evaluator.Models.WrappedTypes;

public class WrappedMethod : WrappedEntity
{
    public MethodInfo Value => (MethodInfo)Object;

    public WrappedMethod(MethodInfo value, string name) : base(value, name)
    {
    }
}