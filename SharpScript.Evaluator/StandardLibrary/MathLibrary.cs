using SharpScript.Evaluator.Attributes;

namespace SharpScript.Evaluator.StandardLibrary;

[StandardLibraryModule("math")]
internal static class MathLibrary
{
    private static readonly Random Random = new();

    [StandardLibraryProperty("pi")]
    public static double Pi => Math.PI;

    [StandardLibraryMethod("rand")]
    public static decimal GetRandomNumber(decimal l, decimal r)
    {
        var ll = (int)l;
        var rr = (int)r;

        return Random.Next(ll, rr);
    }
}