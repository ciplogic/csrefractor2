using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;
using NativeSharp.Optimizations.DeadCodeElimination;

namespace NativeSharp.Optimizations.GotosOptimizations;

public class GotoOpsOptimizationBase : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var indicesToRemove = GotosIndicesToNextLine(cilNativeMethod);

        bool result = false;

        if (indicesToRemove.Length != 0)
        {
            CilMethodExtensions.RemoveIndices(cilNativeMethod, indicesToRemove.ToArray());
            result = true;
        }
        
        var labelsToRemove = LabelsToRemove(cilNativeMethod);
        if (labelsToRemove.Length != 0)
        {
            CilMethodExtensions.RemoveIndices(cilNativeMethod, labelsToRemove);
            result = true;
        }

        return result;
    }

    static int[] LabelsToRemove(CilNativeMethod cilNativeMethod)
    {
        var labels = LabelsAtRows(cilNativeMethod);
        var referencedLabels = ReferencedJumps(cilNativeMethod);
        foreach (int referencedLabel in referencedLabels)
        {
            labels.Remove(referencedLabel);
        }
        return labels.Values.ToArray();
    }
    
    static Dictionary<int, int> LabelsAtRows(CilNativeMethod cilNativeMethod)
    {
        Dictionary<int, int> result = [];
        var ops = cilNativeMethod.Instructions;
        for (var index = 0; index < ops.Length; index++)
        {
            var op = ops[index];
            if (op is LabelOp labelOp)
            {
                result[labelOp.Offset] = index;

            }
        }

        return result;

    }

    static HashSet<int> ReferencedJumps(CilNativeMethod cilNativeMethod)
    {
        HashSet<int> result = [];
        var ops = cilNativeMethod.Instructions;
        foreach (var op in ops)
        {
            if (!op.IsJumpOp())
            {
                continue;
            }

            if (op is GotoOp gotoOp)
            {
                result.Add(gotoOp.Offset);
                continue;
            }

            if (op is BranchOp branchOp)
            {
                result.Add(branchOp.Offset);
            }
        }

        return result;
    }

    private static int[] GotosIndicesToNextLine(CilNativeMethod cilNativeMethod)
    {
        var indicesToRemove = new List<int>();
        var ops = cilNativeMethod.Instructions;
        for (var index = 0; index < ops.Length - 1; index++)
        {
            var op = ops[index];
            if (ops[index] is not GotoOp gotoOp)
            {
                continue;
            }

            if (ops[index + 1] is not LabelOp labelOp)
            {
                continue;
            }

            if (gotoOp.Offset != labelOp.Offset)
            {
                continue;
            }

            indicesToRemove.Add(index);
        }

        return indicesToRemove.ToArray();
    }
}