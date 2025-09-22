using NativeSharp.Operations.Common;

namespace NativeSharp.Operations.Vars;

public abstract class Variable : IValueExpression
{
    public Type ExpressionType { get; set; }

    public string Code()
    {
        return GenCodeImpl();
    }

    public abstract string GenCodeImpl();
}