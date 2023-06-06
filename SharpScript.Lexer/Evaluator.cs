using SharpScript.Lexer.Helpers;
using SharpScript.Lexer.Models.Ast;
using SharpScript.Lexer.Models.Ast.Assignments;
using SharpScript.Lexer.Models.Ast.Declarations;
using SharpScript.Lexer.Models.Ast.Expressions;

namespace SharpScript.Lexer;

public class Evaluator
{
    private readonly Dictionary<string, object?> _environment;

    public Evaluator()
    {
        _environment = new();
        SetupEnvironment();
    }

    public object? Evaluate(Node node)
    {
        return node switch
        {
            RootNode program => EvaluateProgram(program),
            VariableDeclaration variableDeclaration => EvaluateVariableDeclaration(variableDeclaration),
            VariableAssignment variableAssignment => EvaluateVariableAssignment(variableAssignment),
            NumberExpression numberExpression => EvaluateNumberExpression(numberExpression),
            VariableExpression variableExpression => EvaluateVariableExpression(variableExpression),
            FunctionCallExpression functionCallExpression => EvaluateFunctionCallExpression(functionCallExpression),
            FunctionCall functionCall => EvaluateFunctionCall(functionCall),
            null => null,
            _ => throw new Exception($"Don't know how to evaluate {node.GetType().Name}")
        };
    }
    
    private object? EvaluateProgram(RootNode program)
    {
        object? result = null;
        foreach (var statement in program.Statements)
        {
            result = Evaluate(statement);
        }

        return result;
    }

    private object? EvaluateFunctionCall(FunctionCall functionCall)
    {
        var funcName = functionCall.Name;


        ThrowHelper.ThrowIfNotCallable(_environment, funcName);

        var del = _environment[funcName] as Delegate;

        var args = EvaluateCallArguments(functionCall.Values?.ToArray() ?? Array.Empty<NodeExpression>());

        return del!.DynamicInvoke(new object[] { args });
    }
    
    private object? EvaluateFunctionCallExpression(FunctionCallExpression functionCallExpression)
    {
        return EvaluateFunctionCall(functionCallExpression.FunctionCall);
    }

    private object? EvaluateVariableAssignment(VariableAssignment variableAssignment)
    {
        var leftName = variableAssignment.Name;
        var rightName = variableAssignment.Value.Value;

        ThrowHelper.ThrowIfVariableNotDeclared(_environment, leftName);
        ThrowHelper.ThrowIfVariableNotDeclared(_environment, rightName);

        var value = Evaluate(variableAssignment.Value);
        _environment[variableAssignment.Name] = value;
        return value;
    }

    private object? EvaluateVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        var value = Evaluate(variableDeclaration.Value!);
        _environment[variableDeclaration.Name] = value;
        return value;
    }

    private static object? EvaluateNumberExpression(NodeExpression numberExpression)
    {
        return decimal.Parse(numberExpression.Value);
    }

    private object? EvaluateVariableExpression(NodeExpression variableExpression)
    {
        return _environment[variableExpression.Value];
    }

    private object?[] EvaluateCallArguments(IEnumerable<NodeExpression> args)
    {
        return args.Select(Evaluate).ToArray();
    }

    private void SetupEnvironment()
    {
        _environment["print"] = Print;
        _environment["rand"] = GetRandomNumber;
    }

    private static void Print(params object[] args)
    {
        Console.WriteLine(string.Join(" ", args));
    }

    private static readonly Random Random = new();
    private static decimal GetRandomNumber(object[] args)
    {
        var l = (decimal)args[0];
        var r = (decimal)args[1];
        
        var ll = (int)l;
        var rr = (int)r;

        return Random.Next(ll, rr);
    }
}