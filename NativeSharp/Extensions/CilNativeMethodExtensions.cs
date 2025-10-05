using NativeSharp.Cha.Resolving;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Extensions;

public static class CilNativeMethodExtensions
{
    
    public static void RemoveIndices(CilNativeMethod cilNativeMethod, int[] indicesToRemove)
    {
        if (indicesToRemove.Length == 0)
        {
            return;
        }

        var newOps = new List<BaseOp>(cilNativeMethod.Instructions.Length - indicesToRemove.Length);
        for (var index = 0; index < cilNativeMethod.Instructions.Length; index++)
        {
            if (!indicesToRemove.Contains(index))
            {
                newOps.Add(cilNativeMethod.Instructions[index]);
            }
        }

        cilNativeMethod.Instructions = newOps.ToArray();
    }

    public static CilNativeMethod[] CilMethodsFromCache()
    {
        var methodCacheValues = MethodResolver.MethodCache.Values.ToArray();
        List<CilNativeMethod> cilMethods = [];
        foreach (var method in methodCacheValues)
        {
            if (method is CilNativeMethod cilNativeMethod)
            {
                cilMethods.Add(cilNativeMethod);
            }
        }

        return cilMethods.ToArray();
    }
}