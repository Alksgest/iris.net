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

        return env.Variables[name]?.Object;
    }
    
    internal static EmbeddedEntityInScope? GetVariableInScope(IEnumerable<ScopeEnvironment> environments, string name)
    {
        var env = environments.LastOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is not declared");
        }

        return env.Variables[name];
    }

    // TODO: add additional type?
    internal static void SetVariableValue(IEnumerable<ScopeEnvironment> environments, string name, object value)
    {
        var env = environments.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is not declared");
        }

        CreateEmbeddedEntityInScope(env, name, value);
    }
    
    internal static void DeclareVariable(IEnumerable<ScopeEnvironment> envs, string name, object? value)
    {
        var environments = envs.ToList();

        var env = environments.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            var lastEnv = environments.Last();
            CreateEmbeddedEntityInScope(lastEnv, name, value);
            return;
        }
        
        //TODO: debug this line
        CreateEmbeddedEntityInScope(env, name, value);//TODO: probably mistake, exception should be thrown
    }
    
    internal static void AddVariableToScope(IEnumerable<ScopeEnvironment> envs, string name, EmbeddedEntityInScope value)
    {
        var environments = envs.ToList();

        var env = environments.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            var lastEnv = environments.Last();
            lastEnv.Variables[name] = value;
            return;
        }
        
        //TODO: debug this line
        CreateEmbeddedEntityInScope(env, name, value);//TODO: probably mistake, exception should be thrown
    }

    private static void CreateEmbeddedEntityInScope(ScopeEnvironment scope, string name, object? value)
    {
        EmbeddedEntityInScope? objectInScope = value switch
        {
            string str => new StringValueInScope(str, name),
            decimal d => new NumberValueInScope(d, name),
            bool b => new PrimitiveValueInScope<bool>(b, name),
            Delegate del => new DelegateInScope(del, name),
            MethodInfo m => new MethodInScope(m, name),
            List<object> l => new ArrayInScope(l, name),
            _ => null
        };
        
        scope.Variables[name] = objectInScope;
    }
}