using NativeSharp.EscapeAnalysis;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.FieldsAndIndexing;

internal class LoadElementOp : LeftOp
{
    public IndexedVariable Array { get; set; }
    public IValueExpression Index { get; set; }

    public LoadElementOp(IndexedVariable resultElement, IndexedVariable array, IValueExpression index)
        : base(resultElement)
    {
        Array = array;
        Index = index;
    }

    public override string GenCode()
    {
        Type type = Left.ExpressionType;
        bool isByRef = !type.IsValueType;
        string escapedGetter = isByRef && Left.EscapeResult == EscapeKind.Local
            ? ".get()"
            : "";
        return $"{Left.Code()} = ((*{Array.Code()})[{Index.Code()}]){escapedGetter};";
    }

    public override BaseOp Clone()
        => new LoadElementOp(Left, Array, Index);
}