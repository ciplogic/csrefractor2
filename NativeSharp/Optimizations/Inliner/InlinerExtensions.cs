using System.Reflection;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
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
        if (mappedCilMethod is null) return false;
        return IsSimpleMethod(mappedCilMethod);
    }

    internal static CilNativeMethod? ResolvedMethod(MethodBase targetMethod)
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