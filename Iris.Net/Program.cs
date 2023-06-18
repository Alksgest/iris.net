using Iris.Net.Evaluator;
using Iris.Net.Lexer;
using Iris.Net.Parser;
using System;

namespace Iris.Net;

public enum IrisCommands
{
    Start = 0,
    Build
}

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

        // if (args.Length == 0)
        // {
        //     ConsoleHelper.SetErrorColor();
        //     Console.WriteLine("No file or command to execute");
        //     ConsoleHelper.ResetColor();
        //     return;
        // }

        var fileOrCommand = "build" ?? args[0];

        var isCommand = Enum
            .GetNames(typeof(IrisCommands))
            .Select(el => el.ToLower())
            .Contains(fileOrCommand);

        if (isCommand)
        {
            _ = Enum.TryParse(string.Concat(fileOrCommand[..1].ToUpper(), fileOrCommand.AsSpan(1)),
                out IrisCommands command);

            switch (command)
            {
                case IrisCommands.Start:
                    break;
                case IrisCommands.Build:
                    ProjectBuilder.Build();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            var tokenizer = new Tokenizer();

            var fileContent = File.ReadAllText(fileOrCommand);

            var tokens = tokenizer.Process(fileContent);
            var parser = new TokensParser(tokens);
            var tree = parser.ParseTokens();
            var evaluator = new ProgramEvaluator();
            evaluator.Evaluate(tree);
        }
    }
}