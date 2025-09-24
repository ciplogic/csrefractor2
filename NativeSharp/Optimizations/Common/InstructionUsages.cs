using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.Common;

static class InstructionUsages
{
    public static bool IsJumpOp(this BaseOp op) 
        => op switch
        {
            LabelOp or OffsetOp => true,
            _ => false
        };
    public static IEnumerable<string> GetUsagesOf(BaseOp instruction)
    {
        return instruction switch
        {
            AssignOp assignOp => [assignOp.Expression.VarCode()],
            CallOp callOp => callOp.Args.Select(arg => arg.VarCode()),
            CallReturnOp callReturnOp => callReturnOp.Args.Select(arg => arg.VarCode()),
            StoreElementOp storeElementOp =>
            [
                storeElementOp.ArrPtr.VarCode(),
                storeElementOp.Index.VarCode(),
                storeElementOp.ValueToSet.VarCode()
            ],
            LoadElementOp loadElementOp => [loadElementOp.Array.VarCode(), loadElementOp.Index.VarCode()],
            StoreFieldOp storeFieldOp => [storeFieldOp.ThisPtr.VarCode(), storeFieldOp.ValueToSet.VarCode()],
            LoadFieldOp loadFieldOp => [loadFieldOp.ThisPtr.VarCode()],
            LdLenOp ldLen => [ldLen.Right.VarCode()],
            NewArrayOp newArray => [newArray.Count.VarCode()],
            BranchOp branchOperation => [branchOperation.Condition.VarCode()],
            ConvOp convOp => [convOp.RightSideVar.VarCode()],
            BinaryOp binaryOp => [binaryOp.LeftExpression.VarCode(), binaryOp.RightExpression.VarCode()],
            UnaryOp unaryOp => [unaryOp.LeftExpression.VarCode()],
            RetOp retOp => [retOp.ValueExpression.VarCode()],
            _ => []
        };
    }

    public static string[] GetUsagesOfArr(this BaseOp instruction)
        => GetUsagesOf(instruction)
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();

    public static bool RefreshLocalVariables(this CilNativeMethod cilNativeMethod)
    {
        var usages = cilNativeMethod.Instructions.GetUsagesOfOps();
        var oldLocalCount = cilNativeMethod.Locals.Length;
        var vars = cilNativeMethod.Locals
            .Where(localVar => usages.Contains(localVar.Code()))
            .ToArray();
        cilNativeMethod.Locals = vars;
        return vars.Length != oldLocalCount;
    }
    public static HashSet<string> GetUsagesOfOps(this IEnumerable<BaseOp> instructions)
    {
        HashSet<string> result = [];
        foreach (BaseOp instruction in instructions)
        {
            var usagesArr = instruction.GetUsagesOfArr();
            foreach (string usage in usagesArr)
            {
                result.Add(usage);
            }
        }
        return result;
    }

    static string VarCode(this IValueExpression? valueExpression)
    {
        if (valueExpression is null)
        {
            return string.Empty;
        }

        if (valueExpression is not IndexedVariable)
        {
            return string.Empty;
        }

        return valueExpression.Code();
    }
}