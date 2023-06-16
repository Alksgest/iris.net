using System.Reflection;
using SharpScript.Evaluator.Commands;
using SharpScript.Evaluator.Helpers;
using SharpScript.Evaluator.Models;
using SharpScript.Evaluator.StandardLibrary;
using SharpScript.Parser.Models.Ast;
using SharpScript.Parser.Models.Ast.Assignments;
using SharpScript.Parser.Models.Ast.Declarations;
using SharpScript.Parser.Models.Ast.Expressions;

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
            FunctionCall functionCall => EvaluateFunctionCall(functionCall, envs),
            BinaryExpression { Operator: "." } binaryExpression => EvaluateDotOperator(binaryExpression, envs),
            BinaryExpression binaryExpression => EvaluateBinaryExpression(binaryExpression, envs),
            UnaryExpression unaryExpression => EvaluateUnaryExpression(unaryExpression, envs),
            ArrayExpression arrayExpression => EvaluateArrayExpression(arrayExpression, envs),
            BreakableScopeNode breakableScopeNode => EvaluateBreakableScopeNode(breakableScopeNode, envs),
            ScopedNode scopedNode => EvaluateScopedNode(scopedNode, envs),
            ConditionalExpression conditionalExpression => EvaluateConditionalExpression(conditionalExpression, envs),
            WhileExpression whileExpression => EvaluateWhileExpression(whileExpression, envs),
            FunctionDeclaration functionDeclaration => EvaluateFunctionDeclaration(functionDeclaration, envs),
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

        EmbeddedEntityInScope leftValueInScope = left switch
        {
            List<object> list => new ArrayInScope(list),
            Dictionary<string, object> dict => new ObjectInScope(dict),
            string str => new StringValueInScope(str),
            decimal d => new NumberValueInScope(d),
            _ => throw new ArgumentOutOfRangeException()
        };

        return binaryExpression.Right switch
        {
            FunctionCallExpression functionCallExpression => HandleFunctionCallExpression(leftValueInScope,
                functionCallExpression, envs),
            VariableExpression expression => HandleVariableExpression(leftValueInScope, expression),
            _ => null
        };
    }

    private object? HandleFunctionCallExpression(
        EmbeddedEntityInScope leftValueInScope,
        FunctionCallExpression functionCallExpression,
        List<ScopeEnvironment> envs)
    {
        var args = EvaluateCallArguments(
            functionCallExpression.FunctionCall.Values?.ToArray() ?? Array.Empty<NodeExpression>(),
            envs);

        if (leftValueInScope is ObjectInScope objectInScope)
        {
            return CallFunctionWithArguments(objectInScope.Value[functionCallExpression.Value], args, leftValueInScope);
        }

        var method = ObjectHelper.GetNestedMethod(leftValueInScope!, $".{functionCallExpression.Value}",
            leftValueInScope.Name);

        return CallFunctionWithArguments(method, args, leftValueInScope);
    }

    private object? HandleVariableExpression(EmbeddedEntityInScope leftValueInScope, VariableExpression expression)
    {
        if (leftValueInScope is ObjectInScope objectInScope)
        {
            return objectInScope.Value[expression.Value];
        }
        
        return ObjectHelper.GetPropertyValue(leftValueInScope, $".{leftValueInScope.Name}", expression.Value);
    }

    private object? EvaluateFunctionDeclaration(
        FunctionDeclaration functionDeclaration,
        List<ScopeEnvironment> envs)
    {
        var func = (object[] inputArgs) => DeclareFunction(inputArgs, functionDeclaration, envs);
        EnvironmentHelper.DeclareVariable(envs, functionDeclaration.Name, func);
        return null;
    }

    private object? DeclareFunction(
        object[] inputArgs,
        FunctionDeclaration functionDeclaration,
        IEnumerable<ScopeEnvironment> envs)
    {
        var args = functionDeclaration.Arguments;

        var scope = new ScopeEnvironment($"Local_{functionDeclaration.Name}");
        for (var i = 0; i < args.Count; ++i)
        {
            var variableName = args[i].Value;
            EnvironmentHelper.DeclareVariable(new[] { scope }, variableName, inputArgs[i]);
        }

        var totalScope = new List<ScopeEnvironment>(envs) { scope };
        var obj = Evaluate(functionDeclaration.ScopedNode, totalScope);

        return obj;
    }

    private object? EvaluateFunctionCallExpression(
        FunctionCallExpression functionCallExpression,
        List<ScopeEnvironment> environments)
    {
        return EvaluateFunctionCall(functionCallExpression.FunctionCall, environments);
    }

    private object? EvaluateFunctionCall(FunctionCall functionCall, List<ScopeEnvironment> environments)
    {
        var funcName = functionCall.Name;

        ThrowHelper.ThrowIfNotCallable(environments, funcName);

        var func = EnvironmentHelper.GetVariableValue(environments, funcName);

        var args = EvaluateCallArguments(functionCall.Values?.ToArray() ?? Array.Empty<NodeExpression>(), environments);

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
                var prms = argsArray[(parameterInfos.Length - 1)..args.Count];
                var newArgs = new object[normalArguments.Length + 1];

                normalArguments.CopyTo(newArgs, 0);
                newArgs[^1] = prms;

                return method.Invoke(self, newArgs);
            }

            return method.Invoke(self, argsArray);
        }

        // This mean declared function from source code
        var del = func as Delegate;

        return del?.DynamicInvoke(new object?[] { args.ToArray() });
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