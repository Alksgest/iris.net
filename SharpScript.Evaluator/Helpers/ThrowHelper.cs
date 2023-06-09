namespace SharpScript.Evaluator.Helpers;

public static class ThrowHelper
{
    internal static void ThrowIfVariableNotDeclared(IEnumerable<ScopeEnvironment> environments, string name)
    {
        if (environments.Any(env => env.Variables.ContainsKey(name)))
        {
            return;
        }

        throw new Exception($"Variable {name} is not declared");
    }

    internal static void ThrowIfVariableDeclared(IEnumerable<ScopeEnvironment> environments, string name)
    {
        if (environments.Any(env => env.Variables.ContainsKey(name)))
        {
            throw new Exception($"Variable {name} is declared");
        }
    }

    internal static void ThrowIfNotCallable(List<ScopeEnvironment> environments, string name)
    {
        ThrowIfVariableNotDeclared(environments, name);

        var value = EnvironmentHelper.GetVariableValue(environments, name);

        if (value is not Delegate)
        {
            throw new Exception($"Variable {name} is not callable");
        }
    }
}