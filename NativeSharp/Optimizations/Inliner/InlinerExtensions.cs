using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.Inliner;

internal static class InlinerExtensions
{
    internal static int[] CalculateIsCandidate(BaseOp[] ops)
    {
        List<int> indicesCalls = [];
        for (int index = 0; index < ops.Length; index++)
        {
            BaseOp op = ops[index];
            switch (op)
            {
                case CallOp callOp:
                    if (IsInlinable(callOp.TargetMethod))
                    {
                        indicesCalls.Add(index);
                    }

                    break;
                case CallReturnOp callReturnOp:
                    if (IsInlinable(callReturnOp.TargetMethod))
                    {
                        indicesCalls.Add(index);
                    }

                    break;
            }
        }

        return indicesCalls.ToArray();
    }

    private static bool IsInlinable(MethodBase callOpTargetMethod)
    {
        CilOperationsMethod? mappedCilMethod = ResolvedMethod(callOpTargetMethod);
        return mappedCilMethod is not null && IsSimpleMethod(mappedCilMethod);
    }

    internal static CilOperationsMethod? ResolvedMethod(MethodBase? targetMethod)
    {
        if (targetMethod is null)
        {
            return null;
        }

        if (!MethodResolver.MethodCache.TryGetValue(targetMethod, out BaseNativeMethod? mappedCilMethod))
        {
            return null;
        }

        return mappedCilMethod as CilOperationsMethod;
    }

    private static bool IsSimpleMethod(CilOperationsMethod cilOperationsMethod)
    {
        BaseOp[] ops = cilOperationsMethod.Operations;
        foreach (BaseOp op in ops)
        {
            if (op is LabelOp || op is BranchOp)
            {
                return false;
            }

            CilOperationsMethod? targetCall = GetTargetCall(op);
            if (targetCall is not null)
            {
                return false;
            }
        }

        return true;
    }

    internal static CilOperationsMethod? GetTargetCall(BaseOp targetOp)
    {
        MethodBase? targetMethod = null;
        if (targetOp is CallOp callOp)
        {
            targetMethod = callOp.TargetMethod;
        }
        else if (targetOp is CallReturnOp callReturnOp)
        {
            targetMethod = callReturnOp.TargetMethod;
        }

        return ResolvedMethod(targetMethod);
    }
}