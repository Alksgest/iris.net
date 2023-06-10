using System.Reflection;

namespace SharpScript.Evaluator.Models;

public class MethodInScope : ObjectInScope
{
    public MethodInfo Value => (MethodInfo)Object;

    public MethodInScope(MethodInfo value, string name) : base(value, name)
    {
    }
}