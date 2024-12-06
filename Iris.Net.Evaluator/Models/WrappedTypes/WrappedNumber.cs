namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedNumber(decimal value, string? name = null) : WrappedPrimitive<decimal>(value, name);