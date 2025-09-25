using System.Reflection;
using Iris.Net.Evaluator.Attributes.Library;
using Iris.Net.Evaluator.Helpers;
using Iris.Net.Evaluator.Models;
using Iris.Net.Evaluator.Models.WrappedTypes;

namespace Iris.Net.Evaluator.StandardLibrary;

public static class StandardLibraryInitializer
{
    private static readonly List<Type> Modules =
    [
        typeof(ConsoleLibrary),
        typeof(MathLibrary)
    ];

    public static void Init(List<ScopeEnvironment> environments)
    {
        var modules = Modules.Where(t => Attribute.IsDefined(t, typeof(StandardLibraryModuleAttributeWithName)));
        foreach (var module in modules)
        {
            var staticMethods = module.GetMethods(BindingFlags.Static | BindingFlags.Public);
            var properties = module.GetProperties(BindingFlags.Static | BindingFlags.Public);

            var methodsToRegister = staticMethods
                .Where(el => Attribute.IsDefined(el, typeof(StandardLibraryMethodAttributeWithName)))
                .ToList();
            var propertiesToCreate = properties
                .Where(el => Attribute.IsDefined(el, typeof(StandardLibraryPropertyAttributeWithName)))
                .ToList();

            var propertyDictionary = new Dictionary<string, object>();

            if (methodsToRegister.Any())
            {
                foreach (var method in methodsToRegister)
                {
                    var methodAnnotation =
                        (method.GetCustomAttribute(typeof(StandardLibraryMethodAttributeWithName)) as
                            StandardLibraryMethodAttributeWithName)!;

                    propertyDictionary[methodAnnotation.Name] = method;
                }
            }

            if (propertiesToCreate.Any())
            {
                foreach (var property in propertiesToCreate)
                {
                    var propertyAnnotation =
                        (property.GetCustomAttribute(typeof(StandardLibraryPropertyAttributeWithName)) as
                            StandardLibraryPropertyAttributeWithName)!;

                    propertyDictionary[propertyAnnotation.Name] = property.GetValue(module)!;
                }
            }

            var moduleName =
                (module.GetCustomAttribute(typeof(StandardLibraryModuleAttributeWithName)) as StandardLibraryModuleAttributeWithName)!
                .Name;

            var objectInScope = new WrappedDictionary(propertyDictionary, moduleName);

            EnvironmentHelper.AddVariableToScope(environments, moduleName, objectInScope);
        }
    }
}