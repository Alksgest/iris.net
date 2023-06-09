using SharpScript.Evaluator.Helpers;
using SharpScript.Parser.Models.Ast;
using SharpScript.Parser.Models.Ast.Assignments;
using SharpScript.Parser.Models.Ast.Declarations;
using SharpScript.Parser.Models.Ast.Expressions;

namespace SharpScript.Evaluator;

public class ScopeEnvironment
{
    public string Name { get; }
    public Guid Id { get; }
    public Dictionary<string, object?> Variables { get; } = new();

    public ScopeEnvironment(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }
}

public class ProgramEvaluator
{
    private readonly ScopeEnvironment _globalEnvironment;

    public ProgramEvaluator()
    {
        _globalEnvironment = new("Global");
        SetupEnvironment();
    }

    public object? Evaluate(Node node, List<ScopeEnvironment>? environments = null)
    {
        var envs = environments ?? new List<ScopeEnvironment> { _globalEnvironment };

        return node switch
        {
            RootNode program => EvaluateProgram(program, envs),
            VariableDeclaration variableDeclaration => EvaluateVariableDeclaration(variableDeclaration, envs),
            VariableAssignment variableAssignment => EvaluateVariableAssignment(variableAssignment, envs),
            NumberExpression numberExpression => EvaluateNumberExpression(numberExpression, envs),
            BooleanExpression booleanExpression => EvaluateBooleanExpression(booleanExpression, envs),
            StringExpression stringExpression => EvaluateStringExpression(stringExpression, envs),
            VariableExpression variableExpression => EvaluateVariableExpression(variableExpression, envs),
            FunctionCallExpression functionCallExpression =>
                EvaluateFunctionCallExpression(functionCallExpression, envs),
            FunctionCall functionCall => EvaluateFunctionCall(functionCall, envs),
            BinaryExpression binaryExpression => EvaluateBinaryExpression(binaryExpression, envs),
            UnaryExpression unaryExpression => EvaluateUnaryExpression(unaryExpression, envs),
            ScopedNode scopedNode => EvaluateScopedNode(scopedNode, envs),
            ConditionalExpression conditionalExpression => EvaluateConditionalExpression(conditionalExpression, envs),
            null => null,
            _ => throw new Exception($"Don't know how to evaluate {node.GetType().Name}")
        };
    }
    
    private object? EvaluateProgram(RootNode program, List<ScopeEnvironment> environments)
    {
        object? result = null;
        foreach (var statement in program.Statements)
        {
            result = Evaluate(statement, environments);
        }

        return result;
    }

    private object? EvaluateScopedNode(ScopedNode scopedNode, List<ScopeEnvironment> environments)
    {
        object? result = null;

        var localEnv = new ScopeEnvironment($"Local_{environments.Count}");

        var newEnvs = new List<ScopeEnvironment>(environments) { localEnv };

        foreach (var statement in scopedNode.Statements)
        {
            result = Evaluate(statement, newEnvs);
        }

        newEnvs.Clear(); // TODO: we need totally clear scope env

        return result;
    }

    private object? EvaluateConditionalExpression(
        ConditionalExpression conditionalExpression,
        List<ScopeEnvironment> environments)
    {
        var condition = Evaluate(conditionalExpression.Condition, environments);
        if (condition != null)
        {
            return Evaluate(conditionalExpression.True, environments);
        }

        if (conditionalExpression.False != null)
        {
            return Evaluate(conditionalExpression.False, environments);
        }

        return null;
    }
    
    private object? EvaluateUnaryExpression(UnaryExpression unaryExpression, List<ScopeEnvironment> envs)
    {
        var left = Evaluate(unaryExpression.Left)!;
        
        object result = unaryExpression.Operator switch
        {
            "-" => MathHelper.UnaryMinus(left),
            "!" => MathHelper.LogicalNot(left),
            _ => throw new ArgumentException($"{unaryExpression.Operator} is not accepted unary operator")
        };

        return result;
    }

    private object? EvaluateBinaryExpression(BinaryExpression binaryExpression, List<ScopeEnvironment> environments)
    {
        var left = Evaluate(binaryExpression.Left, environments);
        var right = Evaluate(binaryExpression.Right, environments);

        var result = binaryExpression.Operator switch
        {
            "+" => MathHelper.Sum(left, right),
            "-" => MathHelper.Subtract(left, right),
            "*" => MathHelper.Multiply(left, right),
            "/" => MathHelper.Divide(left, right),
            _ => throw new ArgumentException($"{binaryExpression.Operator} is not accepted binary operator")
        };

        return result;
    }

    private object? EvaluateFunctionCall(FunctionCall functionCall, List<ScopeEnvironment> environments)
    {
        var funcName = functionCall.Name;

        ThrowHelper.ThrowIfNotCallable(environments, funcName);

        var func = EnvironmentHelper.GetVariableValue(environments, funcName);
        var del = func as Delegate;

        var args = EvaluateCallArguments(functionCall.Values?.ToArray() ?? Array.Empty<NodeExpression>(), environments);

        return del!.DynamicInvoke(new object[] { args });
    }

    private object? EvaluateFunctionCallExpression(
        FunctionCallExpression functionCallExpression,
        List<ScopeEnvironment> environments)
    {
        return EvaluateFunctionCall(functionCallExpression.FunctionCall, environments);
    }

    private object? EvaluateVariableAssignment(
        VariableAssignment variableAssignment,
        List<ScopeEnvironment> environments)
    {
        var leftName = variableAssignment.Name;

        ThrowHelper.ThrowIfVariableNotDeclared(environments, leftName);

        var value = Evaluate(variableAssignment.Value, environments);

        EnvironmentHelper.SetVariableValue(environments, variableAssignment.Name, value);
        return value;
    }

    private object? EvaluateVariableDeclaration(
        VariableDeclaration variableDeclaration,
        List<ScopeEnvironment> environments)
    {
        ThrowHelper.ThrowIfVariableDeclared(environments, variableDeclaration.Name);

        var value = Evaluate(variableDeclaration.Value!, environments);

        EnvironmentHelper.DeclareVariable(environments, variableDeclaration.Name, value);
        return value;
    }

    private static decimal EvaluateNumberExpression(
        PrimaryExpression numberExpression,
        List<ScopeEnvironment> environments)
    {
        return decimal.Parse(numberExpression.Value);
    }
    
    private static bool EvaluateBooleanExpression(
        PrimaryExpression booleanExpression,
        List<ScopeEnvironment> environments)
    {
        return bool.Parse(booleanExpression.Value);
    }
    
    private static string EvaluateStringExpression(
        PrimaryExpression stringExpression,
        List<ScopeEnvironment> environments)
    {
        return stringExpression.Value[1..^1];
    }

    private object? EvaluateVariableExpression(
        PrimaryExpression variableExpression,
        IReadOnlyCollection<ScopeEnvironment> environments)
    {
        ThrowHelper.ThrowIfVariableNotDeclared(environments, variableExpression.Value);

        var value = EnvironmentHelper.GetVariableValue(environments, variableExpression.Value);
        return value;
    }

    private object?[] EvaluateCallArguments(IEnumerable<NodeExpression> args, List<ScopeEnvironment> environments)
    {
        return args.Select((arg) => Evaluate(arg, environments)).ToArray();
    }

    // TODO: move setup of global end to class with annotation
    private void SetupEnvironment()
    {
        _globalEnvironment.Variables["print"] = Print;
        _globalEnvironment.Variables["rand"] = GetRandomNumber;
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