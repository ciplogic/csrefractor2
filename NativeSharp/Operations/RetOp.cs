using NativeSharp.Operations.Common;

namespace NativeSharp.Operations;

internal class RetOp(IValueExpression? valueExpression) : BaseOp
{
    public IValueExpression? ValueExpression { get; set; } = valueExpression;

    public override string GenCode()
        => ValueExpression is null
            ? ""
            : $"return {ValueExpression.Code()};";
}