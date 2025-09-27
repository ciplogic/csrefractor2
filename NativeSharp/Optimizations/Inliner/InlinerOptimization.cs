using System.Reflection;
using NativeSharp.Lib;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.DeadCodeElimination;
using NativeSharp.Resolving;

namespace NativeSharp.Optimizations.Inliner;

public class InlinerOptimization : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var ops = cilNativeMethod.Instructions;
        var rowsWithCalls = CalculateIsCandidate(ops);

        if (rowsWithCalls.Length == 0)
        {
            return false;
        }

        foreach (var row in rowsWithCalls)
        {
            if (TryInline(cilNativeMethod, row))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryInline(CilNativeMethod cilNativeMethod, int row)
    {
        var op = cilNativeMethod.Instructions[row];
        if (op is CallOp callOp)
        {
            return TryInlineCallOp(cilNativeMethod, row, callOp);
        }

        return false;
    }

    private bool TryInlineCallOp(CilNativeMethod cilNativeMethod, int row, CallOp callOp)
    {
        var targetToInline = ResolvedMethod(callOp.TargetMethod)!;
        var targetOps = targetToInline.Instructions;

        if (targetOps.Length == 1)
        {
            //this is empty method or discarded getter
            CilMethodExtensions.RemoveIndices(cilNativeMethod, [row]);
            return true;
        }

        return false;

    }

    private static int[] CalculateIsCandidate(BaseOp[] ops)
    {
        List<int> indicesCalls = [];
        for (var index = 0; index < ops.Length; index++)
        {
            var op = ops[index];
            switch (op)
            {
                case CallOp callOp:
                    if (callOp.CallType == CallType.Static)
                    {
                        if (IsInlinable(callOp.TargetMethod))
                        {
                            indicesCalls.Add(index);
                        }
                    }

                    break;
                case CallReturnOp callReturnOp:
                    if (callReturnOp.CallType == CallType.Static)
                    {
                        if (IsInlinable(callReturnOp.TargetMethod))
                        {
                            indicesCalls.Add(index);
                        }
                    }

                    break;
            }
        }

        return indicesCalls.ToArray();
    }

    private static bool IsInlinable(MethodBase callOpTargetMethod)
    {
        CilNativeMethod? mappedCilMethod = ResolvedMethod(callOpTargetMethod);
        if (mappedCilMethod is not null)
        {
            if (IsSimpleMethod(mappedCilMethod))
            {
                return true;
            }
        }

        return false;
    }

    private static CilNativeMethod? ResolvedMethod(MethodBase targetMethod)
    {
        var mappedCilMethod = MethodResolver.MethodCache[targetMethod];
        if (mappedCilMethod is CilNativeMethod cilNativeMethod)
        {
            return cilNativeMethod;
        }

        return null;
    }

    private static bool IsSimpleMethod(CilNativeMethod cilNativeMethod)
    {
        var ops = cilNativeMethod.Instructions;
        foreach (var op in ops)
        {
            if (op is CallOp || op is CallReturnOp || op is LabelOp || op is BranchOp)
            {
                return false;
            }
        }

        return true;
    }
}