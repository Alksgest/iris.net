using System.Reflection;
using SharpScript.Evaluator.Attributes;
using SharpScript.Evaluator.Models;

namespace SharpScript.Evaluator.Helpers;

internal static class ObjectHelper
{
    internal static object? GetPropertyValue(ObjectInScope obj, string path, string rootName)
    {
        var type = obj.GetType();
        var properties = type
            .GetProperties()
            .Where(el => Attribute.IsDefined(el, typeof(NestedPropertyAttribute)))
            .ToList();

        var splitted = path.Split(".");

        if (properties.Count == 0)
        {
            throw new ArgumentException($"There are no known properties in object {rootName}");
        }
        
        // TODO: supports only one level depth
        // TODO: add support of any depth in future

        var propertyName = splitted[1];

        var property = properties.SingleOrDefault(el =>
        {
            var attr = el.GetCustomAttribute(typeof(NestedPropertyAttribute)) as NestedPropertyAttribute;
            return attr?.Name == propertyName;
        });

        if (property == null)
        {
            throw new ArgumentException($"There is no known property {propertyName} in object {rootName}");
        }

        return property.GetValue(obj);
    }
    
    internal static MethodInfo GetNestedMethod(ObjectInScope obj, string path, string rootName)
    {
        var type = obj.GetType();
        var methods = type
            .GetMethods()
            .Where(el => Attribute.IsDefined(el, typeof(NestedMethodAttribute)))
            .ToList();

        var splitted = path.Split(".");

        if (methods.Count == 0)
        {
            throw new ArgumentException($"There are no known methods in object {rootName}");
        }
        
        // TODO: supports only one level depth
        // TODO: add support of any depth in future

        var methodName = splitted[1];

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