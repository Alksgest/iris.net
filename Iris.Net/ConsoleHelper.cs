namespace Iris.Net;

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