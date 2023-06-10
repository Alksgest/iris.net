using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.StandardLibrary;

[StandardLibraryModule(Name = "console")]
internal static class ConsoleLibrary
{
    private static readonly Random Random = new();
    
    [StandardLibraryMethod(Name = "print")] // should be log
    public static void Print(params object[] args)
    {
        Console.WriteLine(string.Join(" ", args));
    }
}