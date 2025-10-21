using NativeSharp.Cha.Resolving;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Extensions;

public static class CilNativeMethodExtensions
{
    public static void RemoveIndices(this CilOperationsMethod cilOperationsMethod, int[] indicesToRemove)
    {
        if (indicesToRemove.Length == 0)
        {
            return;
        }

        List<BaseOp> newOps = new List<BaseOp>(cilOperationsMethod.Operations.Length - indicesToRemove.Length);
        for (int index = 0; index < cilOperationsMethod.Operations.Length; index++)
        {
            if (!indicesToRemove.Contains(index))
            {
                newOps.Add(cilOperationsMethod.Operations[index]);
            }
        }

        cilOperationsMethod.Operations = newOps.ToArray();
    }

    public static bool Contains<TOperation>(this Span<BaseOp> cilOperationsMethod) where TOperation : BaseOp
    {
        foreach (BaseOp op in cilOperationsMethod)
        {
            if (op is TOperation)
            {
                return true;
            }
        }
        return false;
    }
    public static bool Contains<TOperation>(this CilOperationsMethod cilOperationsMethod) where TOperation : BaseOp 
        => cilOperationsMethod.Operations.AsSpan().Contains<TOperation>();

    public static CilOperationsMethod[] CilMethodsFromCache()
    {
        NativeMethodBase[] methodCacheValues = MethodResolver.MethodCache.Values.ToArray();
        List<CilOperationsMethod> cilMethods = [];
        foreach (NativeMethodBase method in methodCacheValues)
        {
            if (method is CilOperationsMethod cilNativeMethod)
            {
                cilMethods.Add(cilNativeMethod);
            }
        }

        return cilMethods.ToArray();
    }
    public static NativeMethodBase[] AllMethods() 
        => MethodResolver.MethodCache.Values.ToArray();
}