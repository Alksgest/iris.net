using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.StandardLibrary;

[StandardLibraryModule(Name = "console")]
internal static class ConsoleLibrary
{
    private static readonly Random Random = new();
    
    [StandardLibraryMethod(Name = "print")] // should be log
    public static void Print(params object[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine();
        }

        for (var i = 0; i < args.Length; ++i)
        {
            var el = args[i];
            if (el is List<object> l)
            {
                Console.Write(string.Join(" ", l));
            }
            else
            {
                Console.Write(el);
            }
            
            if (i != args.Length - 1)
            {
                Console.Write(" ");
            }
        }

        Console.WriteLine();
    }
}