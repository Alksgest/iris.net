using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.Json;
using Iris.Net.Evaluator;
using Iris.Net.Helpers;
using Iris.Net.Lexer;
using Iris.Net.Models;
using Iris.Net.Parser;
using Iris.Net.Parser.Models.Ast;

#pragma warning disable SYSLIB0011

namespace Iris.Net;

public static class ProjectBuilder
{
    private const string FileSettingsName = "iris.settings.json";
    private const string BuildFolder = "build";
    private const string BuiltFileSuffix = "cash";

    public static RootNode? Build(string directory)
    {
        var settings = ReadSettings(directory);

        if (settings == null)
        {
            ConsoleHelper.SetErrorColor();
            Console.WriteLine($"There is no {FileSettingsName} in project folder");
            ConsoleHelper.ResetColor();
            return null;
        }

        var mainFile = $"{directory}/{settings.MainFile}";
        var tree = BuildTree(mainFile);

        var buildFolder = $"{directory}/{BuildFolder}";

        if (!Directory.Exists(buildFolder))
        {
            Directory.CreateDirectory(buildFolder);
        }

        using var cashStream = File.OpenWrite($"{buildFolder}/{settings.MainFile}.${BuiltFileSuffix}");

        var hash = ComputeFileHash(cashStream);
        Serialize(tree, cashStream);

        return tree;
    }

    public static void Start(string directory, string? filePath)
    {
        RootNode tree;

        if (filePath != null)
        {
            tree = BuildTree($"{filePath}");
        }
        else
        {
            var settings = ReadSettings(directory);

            if (settings == null)
            {
                ConsoleHelper.SetErrorColor();
                Console.WriteLine($"There is no {FileSettingsName} in project folder");
                ConsoleHelper.ResetColor();
                return;
            }
            
            var cashName = $"{directory}/{BuildFolder}/{settings.MainFile}.cash";

            var isCashExist = File.Exists(cashName);

            tree = !isCashExist ? Build(directory)! : Deserialize(cashName);
        }
        
        var evaluator = new ProgramEvaluator();
        evaluator.Evaluate(tree);
    }

    private static RootNode BuildTree(string filePath)
    {
        var tokenizer = new Tokenizer();

        var fileStream = File.OpenRead(filePath);
        var fileContent = ReadFileStream(fileStream);

        var tokens = tokenizer.Process(fileContent);
        var parser = new TokensParser(tokens);
        return parser.ParseTokens();
    }

    private static string ReadFileStream(Stream stream)
    {
        stream.Position = 0;
        var r = new StreamReader(stream);
        return r.ReadToEnd();
    }

    private static string ComputeFileHash(Stream stream)
    {
        using var mySha256 = SHA256.Create();
        // Compute and print the hash values for each file in directory.
        try
        {
            stream.Position = 0;
            var hashValue = mySha256.ComputeHash(stream);
            PrintByteArray(hashValue);
        }
        catch (IOException e)
        {
            Console.WriteLine($"I/O Exception: {e.Message}");
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine($"Access Exception: {e.Message}");
        }

        return "";
    }

    public static void PrintByteArray(byte[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write($"{array[i]:X2}");
            if ((i % 4) == 3) Console.Write(" ");
        }

        Console.WriteLine();
    }

    private static IrisSettings? ReadSettings(string path)
    {
        var file = new FileInfo($"{path}/{FileSettingsName}");

        if (!file.Exists)
        {
            return null;
        }

        var readOnlySpan = new ReadOnlySpan<byte>(File.ReadAllBytes(file.FullName));

        return JsonSerializer.Deserialize<IrisSettings>(readOnlySpan);
    }

    private static void Serialize(RootNode node, Stream stream)
    {
        stream.Position = 0;

        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, node);

        stream.Position = 0;
    }

    private static RootNode Deserialize(string filename)
    {
        using var fs = File.Open(filename, FileMode.Open);

        var formatter = new BinaryFormatter();

        var obj = formatter.Deserialize(fs);
        var node = obj as RootNode;

        return node!;
    }
}