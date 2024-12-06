using Iris.Net.Helpers;

namespace Iris.Net;

public enum IrisCommand
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

        var path = Environment.CurrentDirectory;

        var (stringCommand, filePath) = PrepareArguments(args);

        // if (stringCommand == null)
        // {
        //     return;
        // }

        // var command = ParseCommand(stringCommand);
        var command = ParseCommand("start");

        if (command == null)
        {
            return;
        }

        switch (command)
        {
            case IrisCommand.Start:
                ProjectBuilder.Start(path, filePath);
                break;
            case IrisCommand.Build:
                ProjectBuilder.Build(path);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static IrisCommand? ParseCommand(string stringCommand)
    {
        var isCommand = Enum
            .GetNames(typeof(IrisCommand))
            .Select(el => el.ToLower())
            .Contains(stringCommand);

        if (!isCommand)
        {
            ConsoleHelper.SetErrorColor();
            Console.WriteLine($"{stringCommand} command is unknown.");
            ConsoleHelper.ResetColor();
            return null;
        }

        _ = Enum.TryParse(string.Concat(stringCommand[..1].ToUpper(), stringCommand.AsSpan(1)),
            out IrisCommand command);

        return command;
    }

    private static (string?, string?) PrepareArguments(string[] args)
    {
        switch (args.Length)
        {
            case 0:
                ConsoleHelper.SetErrorColor();
                Console.WriteLine("No command to execute");
                ConsoleHelper.ResetColor();
                return (null, null);
            case 1:
                return (args[0], null);
            case 2:
            {
                var filePath = args[1];
                var isFile = File.Exists(filePath);

                if (!isFile)
                {
                    ConsoleHelper.SetErrorColor();
                    Console.WriteLine("No file to execute");
                    ConsoleHelper.ResetColor();
                    return (null, null);
                }

                return (args[0], args[1]);
            }
            default:
                throw new Exception("Incorrect number of arguments");
        }
    }
}