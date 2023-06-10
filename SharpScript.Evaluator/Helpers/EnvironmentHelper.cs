using System.Reflection;
using SharpScript.Evaluator.Models;

namespace SharpScript.Evaluator.Helpers;

internal static class EnvironmentHelper
{
    internal static object? GetVariableValue(IEnumerable<ScopeEnvironment> environments, string name)
    {
        var env = environments.LastOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is not declared");
        }

        return env.Variables[name].Object;
    }

    // TODO: add additional type?
    internal static void SetVariableValue(IEnumerable<ScopeEnvironment> environments, string name, object value)
    {
        var env = environments.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is declared");
        }

        CreateObjectInScope(env, name, value);
    }
    
    internal static void DeclareVariable(List<ScopeEnvironment> envs, string name, object? value)
    {
        var env = envs.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            var lastEnv = envs.Last();
            CreateObjectInScope(lastEnv, name, value);
            return;
        }
        
        //TODO: debug this line
        CreateObjectInScope(env, name, value);//TODO: probably mistake, exception should be thrown
    }

    private static void CreateObjectInScope(ScopeEnvironment scope, string name, object? value)
    {
        ObjectInScope? objectInScope = value switch
        {
            string str => new PrimitiveValueInScope<string>(str),
            decimal d => new PrimitiveValueInScope<decimal>(d),
            bool b => new PrimitiveValueInScope<bool>(b),
            Delegate del => new DelegateInScope(del),
            MethodInfo m => new MethodInScope(m),
            _ => null
        };
        
        scope.Variables[name] = objectInScope;
    }
}