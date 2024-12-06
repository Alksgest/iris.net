using System.Reflection;
using Iris.Net.Evaluator.Models;
using Iris.Net.Evaluator.Models.WrappedTypes;

namespace Iris.Net.Evaluator.Helpers;

internal static class EnvironmentHelper
{
    internal static object? GetVariableValue(IEnumerable<ScopeEnvironment> environments, string name)
    {
        var objectInScope = GetVariableInScope(environments, name);
        return objectInScope?.Object;
    }

    private static WrappedEntity? GetVariableInScope(IEnumerable<ScopeEnvironment> environments, string name)
    {
        var env = environments.LastOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is not declared");
        }

        return env.Variables[name];
    }

    internal static void SetVariableValue(IEnumerable<ScopeEnvironment> environments, string name, object? value)
    {
        var env = environments.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is not declared");
        }

        env.Variables[name] = CreateWrappedEntity(name, value);
    }

    internal static void DeclareVariable(IEnumerable<ScopeEnvironment> envs, string name, object? value)
    {
        var variable = CreateWrappedEntity(name, value);
        
        AddVariableToScope(envs, name, variable!);
        // var environments = envs.ToList();
        //
        // // we can declare variable only in the last scope
        // var lastEnv = environments.Last();
        //
        // if (lastEnv.Variables.ContainsKey(name))
        // {
        //     throw new Exception($"Variable {name} is already declared");
        // }
        //
        // lastEnv.Variables[name] = CreateWrappedEntity(name, value);
    }

    internal static void AddVariableToScope(IEnumerable<ScopeEnvironment> envs, string name, WrappedEntity value)
    {
        var environments = envs.ToList();

        var lastEnv = environments.Last();

        if (!lastEnv.Variables.TryAdd(name, value))
        {
            throw new Exception($"Variable {name} is already declared");
        }
    }

    private static WrappedEntity? CreateWrappedEntity(string name, object? value)
    {
        WrappedEntity? wrappedEntity = value switch
        {
            string str => new WrappedString(str, name),
            decimal d => new WrappedNumber(d, name),
            bool b => new WrappedBoolean(b, name),
            Delegate del => new WrappedDelegate(del, name),
            MethodInfo m => new WrappedMethod(m, name),
            List<object> l => new WrappedArray(l, name),
            Dictionary<string, object> dict => new WrappedDictionary(dict),
            _ => null
        };

        return wrappedEntity;
    }
}