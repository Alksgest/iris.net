namespace SharpScript.Evaluator.Commands;

public abstract class Command
{
    
}

public class BreakCommand : Command
{
    
}

public class ReturnCommand : Command
{
    public object? Value { get; }
    public ReturnCommand(object? value)
    {
        Value = value;
    }
}