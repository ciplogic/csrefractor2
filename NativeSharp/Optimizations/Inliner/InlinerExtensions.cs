using System.Reflection;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Resolving;

namespace NativeSharp.Optimizations.Inliner;

internal static class InlinerExtensions
{
    internal static int[] CalculateIsCandidate(BaseOp[] ops)
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
        return mappedCilMethod is not null && IsSimpleMethod(mappedCilMethod);
    }

    internal static CilNativeMethod? ResolvedMethod(MethodBase? targetMethod)
    {
        if (targetMethod is null)
        {
            return null;
        }

        var mappedCilMethod = MethodResolver.MethodCache[targetMethod];
        return mappedCilMethod as CilNativeMethod;
    }

    private static bool IsSimpleMethod(CilNativeMethod cilNativeMethod)
    {
        BaseOp[] ops = cilNativeMethod.Instructions;
        foreach (var op in ops)
        {
            if (op is LabelOp || op is BranchOp)
            {
                return false;
            }

            var targetCall = GetTargetCall(op);
            if (targetCall is not null)
            {
                return false;
            }
        }

        return true;
    }

    internal static CilNativeMethod? GetTargetCall(BaseOp targetOp)
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