using System.Reflection;
using Iris.Net.Evaluator.Attributes.Objects;
using Iris.Net.Evaluator.Models.WrappedTypes;

namespace Iris.Net.Evaluator.Helpers;

internal static class ObjectHelper
{
    internal static object? GetPropertyValue(WrappedEntity obj, string propertyName)
    {
        var type = obj.GetType();
        var properties = type
            .GetProperties()
            .Where(el => Attribute.IsDefined(el, typeof(NestedPropertyAttribute)))
            .ToList();

        if (properties.Count == 0)
        {
            throw new ArgumentException($"There are no known properties in object {obj.Name}");
        }
        
        var property = properties.SingleOrDefault(el =>
        {
            var attr = el.GetCustomAttribute(typeof(NestedPropertyAttribute)) as NestedPropertyAttribute;
            return attr?.Name == propertyName;
        });

        if (property == null)
        {
            throw new ArgumentException($"There is no known property {obj.Name} in object {propertyName}");
        }

        return property.GetValue(obj);
    }
    
    internal static MethodInfo GetNestedMethod(WrappedEntity obj, string methodName, string rootName)
    {
        var type = obj.GetType();
        var methods = type
            .GetMethods()
            .Where(el => Attribute.IsDefined(el, typeof(NestedMethodAttribute)))
            .ToList();

        if (methods.Count == 0)
        {
            throw new ArgumentException($"There are no known methods in object {rootName}");
        }
        
        var method = methods.SingleOrDefault(el =>
        {
            var attr = el.GetCustomAttribute(typeof(NestedMethodAttribute)) as NestedMethodAttribute;
            return attr?.Name == methodName;
        });

        if (method == null)
        {
            throw new ArgumentException($"There is no known property {methodName} in object {rootName}");
        }

        return method;
    }
}