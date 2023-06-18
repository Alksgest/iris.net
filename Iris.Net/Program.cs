using Iris.Net.Evaluator;
using Iris.Net.Lexer;
using Iris.Net.Parser;

namespace Iris.Net;

public static class Program
{
    /// <summary>
    /// Iris.net interpreter
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg);
        }

        if (args.Length == 0)
        {
            Console.WriteLine("No file to execute");
            return;
        }

        var fileName = args[0];

        var tokenizer = new Tokenizer();

        var fileContent = File.ReadAllText(fileName);

        var tokens = tokenizer.Process(fileContent);
        var parser = new TokensParser(tokens); 
        var tree = parser.ParseTokens();
        var evaluator = new ProgramEvaluator();
        evaluator.Evaluate(tree);
    }
}