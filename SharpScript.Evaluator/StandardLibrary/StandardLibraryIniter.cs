using System.Linq.Expressions;
using System.Reflection;
using SharpScript.Evaluator.Attributes;
using SharpScript.Evaluator.Helpers;
using SharpScript.Evaluator.Models;

namespace SharpScript.Evaluator.StandardLibrary;

public static class StandardLibraryInitializer
{
    private static readonly List<Type> Modules = new() { typeof(ConsoleProvider) };

    public static void Init(List<ScopeEnvironment> environments)
    {
        var modules = Modules.Where(t => t.GetCustomAttribute(typeof(StandardLibraryModuleAttribute)) != null);
        foreach (var module in modules)
        {
            var staticMethods = module.GetMethods(BindingFlags.Static | BindingFlags.Public);
            var methodsToRegister = staticMethods
                .Where(el => el.GetCustomAttribute(typeof(StandardLibraryMethodAttribute)) != null)
                .ToList();

            if (!methodsToRegister.Any())
            {
                continue;
            }

            foreach (var method in methodsToRegister)
            {
                // TODO: in future add module as a root object
                var methodAnnotation = method.GetCustomAttribute(typeof(StandardLibraryMethodAttribute)) as StandardLibraryMethodAttribute;
                
                var parameters = method
                    .GetParameters()
                    .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                    .ToArray();
                var call = Expression.Call(method, parameters);
                var lambda = Expression.Lambda(call, parameters);

                var del = lambda.Compile();
                
                EnvironmentHelper.DeclareVariable(environments, methodAnnotation!.Name, del);
            }
        }
    }
}