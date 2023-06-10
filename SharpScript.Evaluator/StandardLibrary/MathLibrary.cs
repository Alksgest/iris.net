using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.StandardLibrary;

[StandardLibraryModule(Name = "math")]
internal static class MathLibrary
{
    private static readonly Random Random = new();
    
    [StandardLibraryMethod(Name = "rand")]
    public static decimal GetRandomNumber(decimal l, decimal r)
    {
        var ll = (int)l;
        var rr = (int)r;

        return Random.Next(ll, rr);
    }
}