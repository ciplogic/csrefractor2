using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

internal class RetOp(IValueExpression? valueExpression) : BaseOp
{
    public IValueExpression? ValueExpression { get; set; } = valueExpression;

    public override string GenCode()
        => ValueExpression is null
            ? ""
            : $"return {ValueExpression.Code()};";

    public override BaseOp Clone()
    {
        return new RetOp(ValueExpression);
    }
}