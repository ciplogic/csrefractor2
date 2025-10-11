using NativeSharp.Cha.Resolving;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Extensions;

public static class CilNativeMethodExtensions
{
    
    public static void RemoveIndices(CilOperationsMethod cilOperationsMethod, int[] indicesToRemove)
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