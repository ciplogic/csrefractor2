using NativeSharp.EscapeAnalysis;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.FieldsAndIndexing;

internal class LoadFieldOp : LeftOp
{
    public IndexedVariable ThisPtr { get; set; }
    public string FieldName { get; }

    public LoadFieldOp(IndexedVariable thisPtr, string fieldName, IndexedVariable left)
        : base(left)
    {
        ThisPtr = thisPtr;
        FieldName = fieldName;
    }

    public override string GenCode()
    {
        Type type = ThisPtr.ExpressionType;
        bool isByRef = !type.IsValueType;
        string derefText = type.IsValueType ? "." : "->";
        string escapedGetter = isByRef && Left.EscapeResult == EscapeKind.Local && !Left.ExpressionType.IsValueType
            ? ".get()"
            : "";
        return $"{Left.Code()} = {ThisPtr.Code()}{derefText}{FieldName}{escapedGetter};";
    }

    public override BaseOp Clone() => new LoadFieldOp(ThisPtr, FieldName, Left);
}