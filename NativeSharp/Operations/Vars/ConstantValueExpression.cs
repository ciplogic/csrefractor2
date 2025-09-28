using NativeSharp.FrontEnd;

namespace NativeSharp.Operations.Vars;

internal class ConstantValueExpression(object value) : IValueExpression
{
    public object? Value { get; } = value;
    public Type ExpressionType { get; set; }

    public string Code()
    {
        if (Value is null)
        {
            return "nullptr";
        }

        if (Value is string text)
        {
            int index = StringPool.Instance.GetIndex(text);
            return $"_str({index})";
        }

        return Value.ToString()!;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ConstantValueExpression constant)
        {
            return false;
        }

        return Value.Equals(constant.Value);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static ConstantValueExpression Create(object value) => new(value)
    {
        ExpressionType = value.GetType()
    };
}