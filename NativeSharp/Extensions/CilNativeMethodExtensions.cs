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

        var newOps = new List<BaseOp>(cilOperationsMethod.Operations.Length - indicesToRemove.Length);
        for (var index = 0; index < cilOperationsMethod.Operations.Length; index++)
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
        var methodCacheValues = MethodResolver.MethodCache.Values.ToArray();
        List<CilOperationsMethod> cilMethods = [];
        foreach (var method in methodCacheValues)
        {
            if (method is CilOperationsMethod cilNativeMethod)
            {
                cilMethods.Add(cilNativeMethod);
            }
        }

        return cilMethods.ToArray();
    }
}