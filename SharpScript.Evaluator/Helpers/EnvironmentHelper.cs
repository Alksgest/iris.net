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

        return env.Variables[name];
    }

    internal static void SetVariableValue(IEnumerable<ScopeEnvironment> environments, string name, object? value)
    {
        var env = environments.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            throw new Exception($"Variable {name} is declared");
        }
        
        env.Variables[name] = value;
    }
    
    internal static void DeclareVariable(List<ScopeEnvironment> envs, string name, object? value)
    {
        var env = envs.SingleOrDefault(env => env.Variables.ContainsKey(name));

        if (env == null)
        {
            var lastEnv = envs.Last();
            lastEnv.Variables[name] = value;
            return;
        }
        
        env.Variables[name] = value; //TODO: probably mistake, exception should be thrown
    }
}