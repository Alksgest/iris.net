namespace SharpScript.Parser.Models.Ast.Expressions;

public class FunctionAssignmentExpression : NodeExpression
{
    public FunctionWrapper Function { get; set; }

    public FunctionAssignmentExpression(FunctionWrapper function) : base(nameof(FunctionAssignmentExpression))
    {
        Function = function;
    }
}