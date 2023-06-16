using System.Reflection;

namespace SharpScript.Evaluator.Models;

public class MethodInScope : EmbeddedEntityInScope
{
    public MethodInfo Value => (MethodInfo)Object;

    public MethodInScope(MethodInfo value, string name) : base(value, name)
    {
    }
}