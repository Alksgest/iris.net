using SharpScript.Evaluator.Attributes;
using SharpScript.Evaluator.Attributes.Library;

namespace SharpScript.Evaluator.StandardLibrary;

[StandardLibraryModuleAttributeWithName("math")]
internal static class MathLibrary
{
    private static readonly Random Random = new();

    [StandardLibraryPropertyAttributeWithName("pi")]
    public static double Pi => Math.PI;

    [StandardLibraryMethodAttributeWithName("rand")]
    public static decimal GetRandomNumber(decimal l, decimal r)
    {
        var ll = (int)l;
        var rr = (int)r;

        return Random.Next(ll, rr);
    }
}