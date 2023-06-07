namespace SharpScript.Lexer.Helpers;

public static class ThrowHelper
{
    internal static void ThrowIfVariableNotDeclared(IReadOnlyDictionary<string, object?> environment, string name)
    {
        if (!environment.ContainsKey(name))
        {
            throw new Exception($"Variable {name} is not declared");
        }
    }
    
    internal static void ThrowIfVariableDeclared(IReadOnlyDictionary<string, object?> environment, string name)
    {
        if (environment.ContainsKey(name))
        {
            throw new Exception($"Variable {name} is declared");
        }
    }
    
    internal static void ThrowIfNotCallable(IReadOnlyDictionary<string, object?> environment, string name)
    {
        ThrowIfVariableNotDeclared(environment, name);

        var value = environment[name];
        if (value is not Delegate)
        {
            throw new Exception($"Variable {name} is not callable");
        }
    }
}