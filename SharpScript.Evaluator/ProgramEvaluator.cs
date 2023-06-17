using System.Reflection;
using SharpScript.Evaluator.Commands;
using SharpScript.Evaluator.Helpers;
using SharpScript.Evaluator.Models;
using SharpScript.Evaluator.Models.WrappedTypes;
using SharpScript.Evaluator.StandardLibrary;
using SharpScript.Parser.Models;
using SharpScript.Parser.Models.Ast;
using SharpScript.Parser.Models.Ast.Assignments;
using SharpScript.Parser.Models.Ast.Declarations;
using SharpScript.Parser.Models.Ast.Expressions;
using SharpScript.Parser.Models.Ast.Expressions.EmbeddedTypes;
using SharpScript.Parser.Models.Ast.Expressions.Statements;

namespace SharpScript.Evaluator;

public class ProgramEvaluator
{
    private readonly ScopeEnvironment _globalEnvironment;

    public ProgramEvaluator()
    {
        _globalEnvironment = new("Global");
        StandardLibraryInitializer.Init(new List<ScopeEnvironment> { _globalEnvironment });
    }

    public object? Evaluate(Node node, List<ScopeEnvironment>? environments = null)
    {
        var envs = environments ?? new List<ScopeEnvironment> { _globalEnvironment };

        return node switch
        {
            RootNode program => EvaluateProgram(program, envs),
            VariableDeclaration variableDeclaration => EvaluateVariableDeclaration(variableDeclaration, envs),
            VariableAssignment variableAssignment => EvaluateVariableAssignment(variableAssignment, envs),
            NumberExpression numberExpression => EvaluateNumberExpression(numberExpression),
            BooleanExpression booleanExpression => EvaluateBooleanExpression(booleanExpression),
            StringExpression stringExpression => EvaluateStringExpression(stringExpression),
            ObjectExpression objectExpression => objectExpression.Value,
            NullExpression _ => null,
            VariableExpression variableExpression => EvaluateVariableExpression(variableExpression, envs),
            FunctionCallExpression functionCallExpression =>
                EvaluateFunctionCallExpression(functionCallExpression, envs),
            FunctionDeclaration functionDeclaration => EvaluateFunctionDeclaration(functionDeclaration, envs),
            FunctionExpression functionAssignmentExpression => EvaluateFunctionAssignmentExpression(
                functionAssignmentExpression, envs),
            BinaryExpression { Operator: "." } binaryExpression => EvaluateDotOperator(binaryExpression, envs),
            BinaryExpression binaryExpression => EvaluateBinaryExpression(binaryExpression, envs),
            UnaryExpression unaryExpression => EvaluateUnaryExpression(unaryExpression, envs),
            ArrayExpression arrayExpression => EvaluateArrayExpression(arrayExpression, envs),
            BreakableScopeNode breakableScopeNode => EvaluateBreakableScopeNode(breakableScopeNode, envs),
            ScopedNode scopedNode => EvaluateScopedNode(scopedNode, envs),
            ConditionalExpression conditionalExpression => EvaluateConditionalExpression(conditionalExpression, envs),
            WhileExpression whileExpression => EvaluateWhileExpression(whileExpression, envs),
            ReturnExpression returnExpression => Evaluate(returnExpression.Expression, envs),
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

    private object? EvaluateScopedNode(ScopedNode scopedNode, IReadOnlyCollection<ScopeEnvironment> environments)
    {
        object? result = null;

        var localEnv = new ScopeEnvironment($"Local_{environments.Count + 1}");

        var newEnvs = new List<ScopeEnvironment>(environments) { localEnv };

        foreach (var statement in scopedNode.Statements)
        {
            result = Evaluate(statement, newEnvs);
            if (statement is ReturnExpression)
            {
                return result;
            }
        }

        newEnvs.Clear();

        return result;
    }

    private object? EvaluateBreakableScopeNode(
        ScopedNode scopedNode,
        IReadOnlyCollection<ScopeEnvironment> environments)
    {
        object? result = null;

        var localEnv = new ScopeEnvironment($"Local_{environments.Count + 1}");

        var newEnvs = new List<ScopeEnvironment>(environments) { localEnv };

        foreach (var statement in scopedNode.Statements)
        {
            if (statement is BreakExpression)
            {
                return new BreakCommand();
            }

            result = Evaluate(statement, newEnvs);
        }

        newEnvs.Clear();

        return result;
    }

    private object? EvaluateWhileExpression(WhileExpression whileExpression, List<ScopeEnvironment> envs)
    {
        // TODO: probably add logic for avoiding 'while(true)'
        var condition = (bool)Evaluate(whileExpression.Condition, envs);
        while (condition)
        {
            var result = Evaluate(whileExpression.Body, envs);
            if (result is BreakCommand)
            {
                break;
            }

            condition = (bool)Evaluate(whileExpression.Condition, envs);
        }

        return condition;
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

    private object EvaluateArrayExpression(ArrayExpression arrayExpression, List<ScopeEnvironment> envs)
    {
        var elements = arrayExpression.Value.Select((el) => Evaluate(el, envs));

        return new List<object?>(elements);
    }

    private object EvaluateUnaryExpression(UnaryExpression unaryExpression, List<ScopeEnvironment> envs)
    {
        var left = Evaluate(unaryExpression.Left, envs)!;

        object result = unaryExpression.Operator switch
        {
            "-" => OperatorsHelper.UnaryMinus(left),
            "!" => OperatorsHelper.LogicalNot(left),
            _ => throw new ArgumentException($"{unaryExpression.Operator} is not accepted unary operator")
        };

        return result;
    }

    private object? EvaluateBinaryExpression(BinaryExpression binaryExpression, List<ScopeEnvironment> environments)
    {
        var left = Evaluate(binaryExpression.Left, environments);
        var right = Evaluate(binaryExpression.Right, environments);

        object result = binaryExpression.Operator switch
        {
            "+" => OperatorsHelper.Sum(left, right),
            "-" => OperatorsHelper.Subtract(left, right),
            "*" => OperatorsHelper.Multiply(left, right),
            "/" => OperatorsHelper.Divide(left, right),
            ">" => OperatorsHelper.GreaterThen(left, right),
            "<" => OperatorsHelper.LessThen(left, right),
            ">=" => OperatorsHelper.GreaterThen(left, right, true),
            "<=" => OperatorsHelper.LessThen(left, right, true),
            "==" => OperatorsHelper.Equal(left, right),
            "!=" => OperatorsHelper.NotEqual(left, right),
            "&&" => OperatorsHelper.LogicalAnd(left, right),
            "||" => OperatorsHelper.LogicalOr(left, right),
            "%" => OperatorsHelper.Reminder(left, right),
            _ => throw new ArgumentException($"{binaryExpression.Operator} is not accepted binary operator")
        };

        return result;
    }

    private object? EvaluateDotOperator(
        BinaryExpression binaryExpression,
        List<ScopeEnvironment> envs)
    {
        var left = Evaluate(binaryExpression.Left, envs)!;

        WrappedEntity leftValue = left switch
        {
            List<object> list => new WrappedArray(list),
            Dictionary<string, object> dict => new WrappedObject(dict),
            string str => new WrappedString(str),
            decimal d => new WrappedNumber(d),
            _ => throw new ArgumentOutOfRangeException()
        };

        return binaryExpression.Right switch
        {
            FunctionCallExpression functionCallExpression => HandleFunctionCallExpression(leftValue,
                functionCallExpression, envs),
            VariableExpression expression => HandleVariableExpression(leftValue, expression),
            _ => null
        };
    }

    private object? HandleFunctionCallExpression(
        WrappedEntity leftValue,
        FunctionCallExpression functionCallExpression,
        List<ScopeEnvironment> envs)
    {
        var args = EvaluateCallArguments(
            functionCallExpression.Values?.ToArray() ?? Array.Empty<NodeExpression>(),
            envs);

        if (leftValue is WrappedObject objectInScope)
        {
            return CallFunctionWithArguments(objectInScope.Value[functionCallExpression.Name], args, leftValue);
        }

        var method = ObjectHelper.GetNestedMethod(
            leftValue!,
            functionCallExpression.Name,
            leftValue.Name);

        return CallFunctionWithArguments(method, args, leftValue);
    }

    private object? HandleVariableExpression(WrappedEntity leftValue, VariableExpression expression)
    {
        if (leftValue is WrappedObject objectInScope)
        {
            return objectInScope.Value[expression.Value];
        }

        return ObjectHelper.GetPropertyValue(leftValue, expression.Value);
    }

    private object? EvaluateFunctionDeclaration(
        FunctionDeclaration functionDeclaration,
        IReadOnlyCollection<ScopeEnvironment> envs)
    {
        var func = (object[] inputArgs) => CreateFunctionBody(inputArgs, functionDeclaration.Function, envs);
        EnvironmentHelper.DeclareVariable(envs, functionDeclaration.Name, func);
        return null;
    }

    private object? EvaluateFunctionAssignmentExpression(
        FunctionExpression functionExpression,
        IReadOnlyCollection<ScopeEnvironment> envs)
    {
        var func = (object[] inputArgs) => CreateFunctionBody(inputArgs, functionExpression.Value, envs);
        return func;
    }

    private object? CreateFunctionBody(
        object[] inputArgs,
        FunctionWrapper function,
        IEnumerable<ScopeEnvironment> envs,
        string? functionName = null)
    {
        var args = function.Arguments;

        var scope = new ScopeEnvironment($"Local_{functionName ?? "anonymous"}");
        for (var i = 0; i < args.Count; ++i)
        {
            var variableName = args[i].Value;
            EnvironmentHelper.DeclareVariable(new[] { scope }, variableName, inputArgs[i]);
        }

        var totalScope = new List<ScopeEnvironment>(envs) { scope };
        var obj = Evaluate(function.Body, totalScope);

        return obj;
    }

    private object? EvaluateFunctionCallExpression(
        FunctionCallExpression functionCallExpression,
        List<ScopeEnvironment> environments)
    {
        var funcName = functionCallExpression.Name;

        ThrowHelper.ThrowIfNotCallable(environments, funcName);

        var func = EnvironmentHelper.GetVariableValue(environments, funcName);

        var args = EvaluateCallArguments(functionCallExpression.Values?.ToArray() ?? Array.Empty<NodeExpression>(),
            environments);

        return CallFunctionWithArguments(func, args);
    }

    private static object? CallFunctionWithArguments(object? func, List<object?> args, object? self = null)
    {
        if (func is MethodInfo method)
        {
            var parameterInfos = method.GetParameters();

            if (parameterInfos.Length == 0)
            {
                return method.Invoke(self, null);
            }

            if (args.Count < parameterInfos.Length)
            {
                for (var i = args.Count; i < parameterInfos.Length; ++i)
                {
                    args.Add(parameterInfos[i].DefaultValue);
                }
            }

            var theLastParam = parameterInfos[^1];
            var paramAttribute = theLastParam
                .CustomAttributes
                .SingleOrDefault(el => el.AttributeType == typeof(ParamArrayAttribute));

            var argsArray = args.ToArray();
            if (paramAttribute != null)
            {
                var normalArguments = argsArray[..(parameterInfos.Length - 1)];
                var @params = argsArray[(parameterInfos.Length - 1)..args.Count];
                var newArgs = new object[normalArguments.Length + 1];

                normalArguments.CopyTo(newArgs, 0);
                newArgs[^1] = @params;

                return method.Invoke(self, newArgs);
            }

            var invokeResult = method.Invoke(self, argsArray);
            return invokeResult;
        }

        // This mean declared function from source code
        var del = func as Delegate;

        var dynamicInvokeResult = del?.DynamicInvoke(new object?[] { args.ToArray() });
        return dynamicInvokeResult;
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

    private static decimal EvaluateNumberExpression(PrimaryExpression<decimal> numberExpression)
    {
        return numberExpression.Value;
    }

    private static bool EvaluateBooleanExpression(PrimaryExpression<bool> booleanExpression)
    {
        return booleanExpression.Value;
    }

    private static string EvaluateStringExpression(PrimaryExpression<string> stringExpression)
    {
        return stringExpression.Value[1..^1];
    }

    private object? EvaluateVariableExpression(
        PrimaryExpression<string> variableExpression,
        IReadOnlyCollection<ScopeEnvironment> environments)
    {
        ThrowHelper.ThrowIfVariableNotDeclared(environments, variableExpression.Value);

        var value = EnvironmentHelper.GetVariableValue(environments, variableExpression.Value);
        return value;
    }

    private List<object?> EvaluateCallArguments(IEnumerable<NodeExpression> args, List<ScopeEnvironment> environments)
    {
        return args.Select((arg) => Evaluate(arg, environments)).ToList();
    }
}