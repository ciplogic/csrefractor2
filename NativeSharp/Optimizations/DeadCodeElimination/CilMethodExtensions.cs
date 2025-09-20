using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.DeadCodeElimination;

public static class CilMethodExtensions
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
}