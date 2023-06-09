using SharpScript.Evaluator;
using SharpScript.Lexer;
using SharpScript.Parser;

namespace SharpScript;

public static class Program
{
    public static void Main(string[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg);
        }

        // if (args.Length == 0)
        // {
        //     Console.WriteLine("No file to execute");
        //     return;
        // }

        var fileName = "main.shs" ?? args[0];

        var tokenizer = new Tokenizer();

        var fileContent = File.ReadAllText(fileName);

        var tokens = tokenizer.Process(fileContent);

        foreach (var token in tokens)
        {
            Console.WriteLine($"{token.Type.ToString()}: {token.Value}");
        }

        var parser = new TokensParser(tokens);
        var tree = parser.ParseTokens();
        
        Console.WriteLine(tree);
        
        var evaluator = new ProgramEvaluator();
        evaluator.Evaluate(tree);
    }
}