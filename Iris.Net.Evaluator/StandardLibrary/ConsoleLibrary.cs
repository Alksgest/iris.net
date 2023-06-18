using Iris.Net.Evaluator.Attributes.Library;

namespace Iris.Net.Evaluator.StandardLibrary;

[StandardLibraryModuleAttributeWithName("console")]
internal static class ConsoleLibrary
{
    [StandardLibraryMethodAttributeWithName("log")] 
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