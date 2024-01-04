namespace Iris.Net.Helpers;

public static class ConsoleHelper
{
    public static void SetErrorColor()
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    
    public static void ResetColor()
    {
        Console.ForegroundColor = ConsoleColor.White;
    }
}