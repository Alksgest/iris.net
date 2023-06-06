using SharpScript.Lexer.Models.Ast;
using SharpScript.Lexer.Models.Ast.Declarations;
using SharpScript.Lexer.Models.Ast.Expressions;

namespace SharpScript.Lexer;

public class Evaluator
{
    private readonly Dictionary<string, object?> _environment = new();

    public object? Evaluate(Node node)
    {
        return node switch
        {
            RootNode program => EvaluateProgram(program),
            VariableDeclaration variableDeclaration => EvaluateVariableDeclaration(variableDeclaration),
            VariableAssignment variableAssignment => EvaluateVariableAssignment(variableAssignment),
            NumberExpression numberExpression => EvaluateNumberExpression(numberExpression),
            VariableExpression variableExpression => EvaluateVariableExpression(variableExpression),
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
    
    private object? EvaluateVariableAssignment(VariableAssignment variableAssignment)
    {
        var leftName = variableAssignment.Name;
        var rightName = variableAssignment.Value.Value;

        ThrowIfVariableNotDeclared(leftName);
        ThrowIfVariableNotDeclared(rightName);

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

    private object? EvaluateNumberExpression(NodeExpression numberExpression)
    {
        return decimal.Parse(numberExpression.Value);
    }
    
    private object? EvaluateVariableExpression(NodeExpression variableExpression)
    {
        return _environment[variableExpression.Value];
    }

    private void ThrowIfVariableNotDeclared(string name)
    {
        if (!_environment.ContainsKey(name))
        {
            throw new Exception($"Variable {name} is not declared");
        }
    }
}