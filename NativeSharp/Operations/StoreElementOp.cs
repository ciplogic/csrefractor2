using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

internal class StoreElementOp(IndexedVariable arrPtr, IValueExpression index, IValueExpression valueToSet) : BaseOp
{
    public IndexedVariable ArrPtr { get; set; } = arrPtr;
    public IValueExpression Index { get; set; } = index;
    public IValueExpression ValueToSet { get; set; } = valueToSet;

    public override string GenCode()
    {
        return $"(*{ArrPtr.Code()})[{Index.Code()}] = {ValueToSet.Code()};";
    }
}