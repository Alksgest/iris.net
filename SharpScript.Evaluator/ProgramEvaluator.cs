using SharpScript.Evaluator.Helpers;
using SharpScript.Parser.Models.Ast;
using SharpScript.Parser.Models.Ast.Assignments;
using SharpScript.Parser.Models.Ast.Declarations;
using SharpScript.Parser.Models.Ast.Expressions;

namespace SharpScript.Evaluator;

public class ProgramEvaluator
{
    private readonly Dictionary<string, object?> _environment;

    public ProgramEvaluator()
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
            StringExpression stringExpression => EvaluateStringExpression(stringExpression),
            VariableExpression variableExpression => EvaluateVariableExpression(variableExpression),
            FunctionCallExpression functionCallExpression => EvaluateFunctionCallExpression(functionCallExpression),
            FunctionCall functionCall => EvaluateFunctionCall(functionCall),
            BinaryExpression binaryExpression => EvaluateBinaryExpression(binaryExpression),
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

    private object? EvaluateBinaryExpression(BinaryExpression binaryExpression)
    {
        var left = Evaluate(binaryExpression.Left);
        var right = Evaluate(binaryExpression.Right);

        var result = binaryExpression.Operator switch
        {
            "+" => MathHelper.Sum(left, right),
            "-" => MathHelper.Subtract(left, right),
            "*" => MathHelper.Multiply(left, right),
            "/" => MathHelper.Divide(left, right),
            _ => throw new ArgumentException("Not accepted operand")
        };

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

        ThrowHelper.ThrowIfVariableNotDeclared(_environment, leftName);

        var value = Evaluate(variableAssignment.Value);
        _environment[variableAssignment.Name] = value;
        return value;
    }

    private object? EvaluateVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        ThrowHelper.ThrowIfVariableDeclared(_environment, variableDeclaration.Name);

        var value = Evaluate(variableDeclaration.Value!);
        _environment[variableDeclaration.Name] = value;
        return value;
    }

    private static object? EvaluateNumberExpression(PrimaryExpression numberExpression)
    {
        return decimal.Parse(numberExpression.Value);
    }

    private static object? EvaluateStringExpression(PrimaryExpression stringExpression)
    {
        return stringExpression.Value[1..^1];
    }

    private object? EvaluateVariableExpression(PrimaryExpression variableExpression)
    {
        ThrowHelper.ThrowIfVariableNotDeclared(_environment, variableExpression.Value);
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