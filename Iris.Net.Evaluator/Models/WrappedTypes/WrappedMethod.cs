using System.Reflection;

namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedMethod(MethodInfo value, string name) : WrappedEntity(value, name)
{
    public MethodInfo Value => (MethodInfo)Object;
}