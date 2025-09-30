using NativeSharp.EscapeAnalysis;

namespace NativeSharp.Operations.Vars;

public abstract class Variable : IValueExpression
{
    public Type ExpressionType { get; set; }
    public EscapeKind EscapeResult { get; set; } = EscapeKind.Unused;

    public string Code()
    {
        return GenCodeImpl();
    }

    public abstract string GenCodeImpl();

    public override bool Equals(object? obj)
    {
        if (obj is Variable var)
        {
            return Code() == var.Code();
        }

        return false;
    }

    public override int GetHashCode()
        => Code().GetHashCode();
}