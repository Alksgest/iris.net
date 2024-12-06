namespace Iris.Net.Evaluator.Models.WrappedTypes;

public class WrappedBoolean(bool value, string? name = null) : WrappedPrimitive<bool>(value, name);