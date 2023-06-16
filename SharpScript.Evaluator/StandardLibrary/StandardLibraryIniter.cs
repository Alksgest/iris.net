using System.Linq.Expressions;
using System.Reflection;
using SharpScript.Evaluator.Attributes;
using SharpScript.Evaluator.Helpers;
using SharpScript.Evaluator.Models;

namespace SharpScript.Evaluator.StandardLibrary;

public static class StandardLibraryInitializer
{
    private static readonly List<Type> Modules = new()
    {
        typeof(ConsoleLibrary),
        typeof(MathLibrary)
    };

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

            var propertyDictionary = new Dictionary<string, object>();

            foreach (var method in methodsToRegister)
            {
                var methodAnnotation =
                    method.GetCustomAttribute(typeof(StandardLibraryMethodAttribute)) as StandardLibraryMethodAttribute;

                propertyDictionary[methodAnnotation!.Name] = method;
            }

            var moduleName =
                (module.GetCustomAttribute(typeof(StandardLibraryModuleAttribute)) as StandardLibraryModuleAttribute)!
                .Name;

            var objectInScope = new ObjectInScope(propertyDictionary, moduleName);

            EnvironmentHelper.AddVariableToScope(environments, moduleName, objectInScope);
        }
    }
}