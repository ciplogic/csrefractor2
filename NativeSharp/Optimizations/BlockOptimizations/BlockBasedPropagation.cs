using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Values;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.BlockOptimizations;

internal class BlockBasedPropagation : BlockBasedOptimization
{
    public override bool OptimizeSegment(ArraySegment<BaseOp> segment)
    {
        Dictionary<IValueExpression, IValueExpression> updates = [];

        var result = false;
        for (var i = 0; i < segment.Count; i++)
        {
            var op = segment[i];
            result |= UpdateKnownOps(op, updates);
            if (op is not AssignOp assignOp)
            {
                continue;
            }

            if (assignOp.Expression is ConstantValueExpression || assignOp.Expression is IndexedVariable)
            {
                updates[assignOp.Left] = assignOp.Expression;
            }
        }

        return result;
    }

    public bool UpdateKnownOps(BaseOp op, Dictionary<IValueExpression, IValueExpression> updates)
        => op switch
        {
            AssignOp assignOp => UpdateExpressionT(assignOp, MatchAssign, updates),
            CallReturnOp callReturnOp => UpdateExpressionT(callReturnOp, MatchCallReturn, updates),
            CallOp callOp => UpdateExpressionT(callOp, MatchCall, updates),
            BranchOp branchOperation => UpdateExpressionT(branchOperation, MatchBranchOp, updates),
            BinaryOp binaryOp => UpdateExpressionT(binaryOp, MatchBinaryOp, updates),
            LoadElementOp loadElementOp => UpdateExpressionT(loadElementOp, MatchLoadElem, updates),
            StoreElementOp storeElementOp => UpdateExpressionT(storeElementOp, MatchStoreElem, updates),
            LoadFieldOp loadFieldOp => UpdateExpressionT(loadFieldOp, MatchLoadField, updates),
            StoreFieldOp storeFieldOp => UpdateExpressionT(storeFieldOp, MapStoreField, updates),
            NewArrayOp newArrayOp => UpdateExpressionT(newArrayOp, MatchNewArrayOp, updates),
            RetOp retOp => UpdateExpressionT(retOp, MatchRetOp, updates),
            _ => false
        };

    private bool MatchRetOp(RetOp op, FromTo fromTo)
    {
        return UpdateExpression(op,
            x => x.ValueExpression,
            (x, v) => x.ValueExpression = v,
            fromTo);
    }

    private bool MatchLoadField(LoadFieldOp op, FromTo fromTo)
        => UpdateExpression(
            op,
            x => x.ThisPtr,
            (x, v) => x.ThisPtr = (IndexedVariable)v,
            fromTo);

    private static bool MapStoreField(StoreFieldOp op, FromTo fromTo)
        => UpdateExpression(op,
            x => x.ThisPtr,
            (x, field) => x.ThisPtr = (IndexedVariable)field,
            fromTo);


    private static bool MatchStoreElem(StoreElementOp storeElementOp, FromTo fromTo)
    {
        var target = UpdateTargetExpression(storeElementOp.ArrPtr, fromTo);
        storeElementOp.ArrPtr = (IndexedVariable)target.Mapped;
        var targetIndex = UpdateTargetExpression(storeElementOp.Index, fromTo);
        storeElementOp.Index = targetIndex.Mapped;
        var targetValue = UpdateTargetExpression(storeElementOp.ValueToSet, fromTo);
        storeElementOp.ValueToSet = targetValue.Mapped;

        return target.Changed || targetIndex.Changed || targetValue.Changed;
    }

    private static bool MatchLoadElem(LoadElementOp op, FromTo fromTo)
    {
        var target = UpdateTargetExpression(op.Array, fromTo);
        op.Array = (IndexedVariable)target.Mapped;

        var targetIndex = UpdateTargetExpression(op.Index, fromTo);
        op.Index = targetIndex.Mapped;
        return target.Changed || targetIndex.Changed;
    }

    private static bool MatchNewArrayOp(NewArrayOp op, FromTo fromTo) =>
        UpdateExpression(op,
            x => x.Count,
            (x, v) => x.Count = v,
            fromTo);

    private static bool MatchBinaryOp(BinaryOp op, FromTo fromTo)
    {
        var leftChanged = UpdateExpression(op,
            x => x.LeftExpression,
            (x, v) => x.LeftExpression = v,
            fromTo);
        var rightChanged = UpdateExpression(op,
            x => x.RightExpression,
            (x, v) => x.RightExpression = v,
            fromTo);
        return leftChanged || rightChanged;
    }

    private bool MatchBranchOp(BranchOp op, FromTo fromTo)
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