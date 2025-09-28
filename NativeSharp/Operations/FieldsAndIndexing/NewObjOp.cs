using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.FieldsAndIndexing;

internal class NewObjOp(IndexedVariable left) : LeftOp(left)
{
    public override string GenCode()
    {
        var expressionTypeName = Left.ExpressionType.Mangle(RefKind.Value);
        return $"{Left.Code()} = new_ref<{expressionTypeName}>();";
    }

    public override BaseOp Clone() => new NewObjOp(Left);
}