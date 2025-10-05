using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Calls;
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
            BinaryOp binaryOp => [binaryOp.LeftExpression.VarCode(), binaryOp.RightExpression.VarCode()],
            UnaryOp unaryOp => [unaryOp.ValueExpression.VarCode()],
            RetOp retOp => [retOp.ValueExpression.VarCode()],
            _ => []
        };
    }

    public static string[] GetUsagesOfArr(this BaseOp instruction)
        => GetUsagesOf(instruction)
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();

    public static bool RefreshLocalVariables(this CilOperationsMethod cilOperationsMethod)
    {
        var usages = cilOperationsMethod.Operations.GetUsagesAndDeclarationsOfOps();
        var oldLocalCount = cilOperationsMethod.Locals.Length;
        var vars = cilOperationsMethod.Locals
            .Where(localVar => usages.Contains(localVar.Code()))
            .ToArray();
        cilOperationsMethod.Locals = vars;
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

    public static HashSet<string> GetUsagesAndDeclarationsOfOps(this IEnumerable<BaseOp> instructions)
    {
        HashSet<string> result = GetUsagesOfOps(instructions);
        foreach (BaseOp op in instructions)
        {
            if (op is LeftOp leftOp)
            {
                result.Add(leftOp.Left.Code());
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

    public static void UpdateUsagesAndDefinitions(BaseOp instruction,
        Dictionary<IValueExpression, IValueExpression> updates)
    {
        UpdateKnownOpUsages(instruction, updates);
        if (instruction is not LeftOp leftOp)
        {
            return;
        }

        if (updates.TryGetValue(leftOp.Left, out var update))
        {
            leftOp.Left = (IndexedVariable)update;
        }
    }

    public static bool UpdateKnownOpUsages(BaseOp op, Dictionary<IValueExpression, IValueExpression> updates)
        => op switch
        {
            AssignOp assignOp => UpdateExpressionT(assignOp, MatchAssign, updates),
            CallReturnOp callReturnOp => UpdateExpressionT(callReturnOp, MatchCallReturn, updates),
            CallOp callOp => UpdateExpressionT(callOp, MatchCall, updates),
            BranchOp branchOperation => UpdateExpressionT(branchOperation, MatchBranchOp, updates),
            BinaryOp binaryOp => UpdateExpressionT(binaryOp, MatchBinaryOp, updates),
            UnaryOp unaryOp => UpdateExpressionT(unaryOp, MatchUnaryOp, updates),
            LoadElementOp loadElementOp => UpdateExpressionT(loadElementOp, MatchLoadElem, updates),
            StoreElementOp storeElementOp => UpdateExpressionT(storeElementOp, MatchStoreElem, updates),
            LoadFieldOp loadFieldOp => UpdateExpressionT(loadFieldOp, MatchLoadField, updates),
            StoreFieldOp storeFieldOp => UpdateExpressionT(storeFieldOp, MapStoreField, updates),
            NewArrayOp newArrayOp => UpdateExpressionT(newArrayOp, MatchNewArrayOp, updates),
            RetOp retOp => UpdateExpressionT(retOp, MatchRetOp, updates),
            _ => false
        };

    private static bool MatchUnaryOp(UnaryOp op, FromTo fromTo)
        => UpdateExpression(op,
            x => x.ValueExpression,
            (x, v) => x.ValueExpression = v,
            fromTo);

    private static bool MatchRetOp(RetOp op, FromTo fromTo)
        => UpdateExpression(op,
            x => x.ValueExpression,
            (x, v) => x.ValueExpression = v,
            fromTo);

    private static bool MatchLoadField(LoadFieldOp op, FromTo fromTo)
        => UpdateExpression(
            op,
            x => x.ThisPtr,
            (x, v) => x.ThisPtr = (IndexedVariable)v,
            fromTo);

    private static bool MapStoreField(StoreFieldOp op, FromTo fromTo)
    {
        var expr1 = UpdateExpression(op,
            x => x.ThisPtr,
            (x, field) => x.ThisPtr = (IndexedVariable)field,
            fromTo);
        var expr2 = UpdateExpression(op,
            x => x.ValueToSet,
            (x, v) => x.ValueToSet = v,
            fromTo);
        return expr1 || expr2;
    }


    private static bool MatchStoreElem(StoreElementOp op, FromTo fromTo)
    {
        var expr1 = UpdateExpression(op,
            x => x.ArrPtr,
            (x, v) => x.ArrPtr = (IndexedVariable)v,
            fromTo);
        var expr2 = UpdateExpression(op,
            x => x.Index,
            (x, v) => x.Index = v,
            fromTo);
        var expr3 = UpdateExpression(op,
            x => x.ValueToSet,
            (x, v) => x.ValueToSet = v,
            fromTo);
        return expr1 || expr2 || expr3;
    }

    private static bool MatchLoadElem(LoadElementOp op, FromTo fromTo)
    {
        var arrayUpdate = UpdateExpression(op,
            x => x.Array,
            (x, v) => x.Array = (IndexedVariable)v,
            fromTo);
        var indexUpdate = UpdateExpression(op,
            x => x.Index,
            (x, v) => x.Index = v,
            fromTo
        );
        return arrayUpdate || indexUpdate;
    }

    private static bool MatchNewArrayOp(NewArrayOp op, FromTo fromTo) =>
        UpdateExpression(op,
            x => x.Count,
            (x, v) => x.Count = v,
            fromTo);

    private static bool MatchBinaryOp(BinaryOp op, FromTo fromTo)
    {
        var leftUpdate = UpdateExpression(op,
            x => x.LeftExpression,
            (x, v) => x.LeftExpression = v,
            fromTo);
        var rightUpdate = UpdateExpression(op,
            x => x.RightExpression,
            (x, v) => x.RightExpression = v,
            fromTo);
        return leftUpdate || rightUpdate;
    }

    private static bool MatchBranchOp(BranchOp op, FromTo fromTo)
        => UpdateExpression(op,
            x => x.Condition,
            (x, v) => x.Condition = v,
            fromTo);

    private static bool MatchAssign(AssignOp op, FromTo fromTo)
        => UpdateExpression(op,
            x => x.Expression,
            (x, v) => x.Expression = v,
            fromTo);

    private static bool MatchCallReturn(CallReturnOp op, FromTo fromTo)
        => UpdateUsagesArguments(op.Args, fromTo);

    private static bool MatchCall(CallOp op, FromTo fromTo)
        => UpdateUsagesArguments(op.Args, fromTo);

    private static bool UpdateUsagesArguments(IValueExpression[] parameters, FromTo fromTo)
    {
        var result = false;
        for (var index = 0; index < parameters.Length; index++)
        {
            var target = UpdateTargetExpression(parameters[index], fromTo);
            parameters[index] = target.Mapped;
            result |= target.Changed;
        }

        return result;
    }

    private static bool UpdateExpressionT<T>(T op, Func<T, FromTo, bool> match,
        Dictionary<IValueExpression, IValueExpression> updates) where T : BaseOp
    {
        var result = false;
        foreach (var kv in updates)
        {
            result |= match(op, new FromTo(kv.Key, kv.Value));
        }

        return result;
    }

    private static bool UpdateExpression<T>(T storeFieldOp, Func<T, IValueExpression> getFieldFunc,
        Action<T, IValueExpression> update,
        FromTo fromTo) where T : BaseOp
    {
        var target = UpdateTargetExpression(getFieldFunc(storeFieldOp), fromTo);
        if (target.Changed)
        {
            update(storeFieldOp, target.Mapped);
        }

        return target.Changed;
    }

    private static (IValueExpression Mapped, bool Changed) UpdateTargetExpression(IValueExpression targetExpression,
        FromTo fromTo)
        => targetExpression == fromTo.From
            ? (fromTo.To, true)
            : (targetExpression, false);

    record struct FromTo(IValueExpression From, IValueExpression To);
}