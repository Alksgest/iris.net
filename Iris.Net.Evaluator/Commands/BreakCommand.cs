namespace Iris.Net.Evaluator.Commands;

public abstract class Command
{
    
}

public class BreakCommand : Command
{
    
}

public class ReturnCommand(object? value) : Command
{
    public object? Value { get; } = value;
}