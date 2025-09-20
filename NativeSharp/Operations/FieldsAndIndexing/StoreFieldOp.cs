using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.FieldsAndIndexing;

internal class StoreFieldOp(IndexedVariable thisPtr, IValueExpression valueToSet, string fieldName)
    : BaseOp
{
    public IndexedVariable ThisPtr { get; set; } = thisPtr;
    public IValueExpression ValueToSet { get; set; } = valueToSet;
    public string FieldName { get; } = fieldName;

    public override string GenCode()
    {
        string derefText = ThisPtr.ExpressionType.IsValueType ? "." : "->";
        return $"{ThisPtr.Code()}{derefText}{FieldName} = {ValueToSet.Code()};";
    }
}