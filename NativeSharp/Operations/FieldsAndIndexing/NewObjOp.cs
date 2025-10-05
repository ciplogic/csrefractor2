using NativeSharp.Cha;
using NativeSharp.CodeGen;
using NativeSharp.EscapeAnalysis;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.FieldsAndIndexing;

internal class NewObjOp(IndexedVariable left) : LeftOp(left)
{
    public override string GenCode()
    {
        var expressionTypeName = Left.ExpressionType.Mangle(RefKind.Value);
        var typeId = ClassHierarchyAnalysis.GetTypeId(Left.ExpressionType);
        if (Left.EscapeResult == EscapeKind.Local)
        {
            var leftCode = Left.Code();
            return
                $"""
                  {expressionTypeName} {leftCode}_instance;
                    {leftCode} = &{leftCode}_instance;  
                 """;
        }
        return $"{Left.Code()} = new_ref<{expressionTypeName}>({typeId});";
    }

    public override BaseOp Clone() => new NewObjOp(Left);
}