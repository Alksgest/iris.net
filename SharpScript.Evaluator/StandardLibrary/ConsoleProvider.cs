using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.StandardLibrary;

[StandardLibraryModule(Name = "console")]
internal static class ConsoleProvider
{
    private static readonly Random Random = new();
    
    [StandardLibraryMethod(Name = "print")] // should be log
    public static void Print(params object[] args)
    {
        Console.WriteLine(string.Join(" ", args));
    }

    [StandardLibraryMethod(Name = "rand")] // TODO: move to the separate class
    // public static decimal GetRandomNumber(decimal l, decimal r)
    public static decimal GetRandomNumber(object[] args)
    {
        var l = (decimal)args[0];
        var r = (decimal)args[1];

        var ll = (int)l;
        var rr = (int)r;

        return Random.Next(ll, rr);
    }
}