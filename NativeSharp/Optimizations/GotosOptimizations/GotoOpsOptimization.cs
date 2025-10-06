using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.GotosOptimizations;

public class GotoOpsOptimization : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        int[] indicesToRemove = GotosIndicesToNextLine(cilOperationsMethod);

        bool result = false;

        if (indicesToRemove.Length != 0)
        {
            CilNativeMethodExtensions.RemoveIndices(cilOperationsMethod, indicesToRemove.ToArray());
            result = true;
        }

        int[] labelsToRemove = LabelsToRemove(cilOperationsMethod);
        if (labelsToRemove.Length != 0)
        {
            CilNativeMethodExtensions.RemoveIndices(cilOperationsMethod, labelsToRemove);
            result = true;
        }

        return result;
    }

    static int[] LabelsToRemove(CilOperationsMethod cilOperationsMethod)
    {
        Dictionary<int, int> labels = LabelsAtRows(cilOperationsMethod);
        HashSet<int> referencedLabels = ReferencedJumps(cilOperationsMethod);
        foreach (int referencedLabel in referencedLabels)
        {
            labels.Remove(referencedLabel);
        }

        return labels.Values.ToArray();
    }

    static Dictionary<int, int> LabelsAtRows(CilOperationsMethod cilOperationsMethod)
    {
        Dictionary<int, int> result = [];
        BaseOp[] ops = cilOperationsMethod.Operations;
        for (int index = 0; index < ops.Length; index++)
        {
            BaseOp op = ops[index];
            if (op is LabelOp labelOp)
            {
                result[labelOp.Offset] = index;
            }
        }

        return result;
    }

    private static HashSet<int> ReferencedJumps(CilOperationsMethod cilOperationsMethod)
    {
        HashSet<int> result = [];
        BaseOp[] ops = cilOperationsMethod.Operations;
        foreach (BaseOp op in ops)
        {
            if (op is JumpToOffset jumpToOp)
            {
                result.Add(jumpToOp.Offset);
            }
        }

        return result;
    }

    private static int[] GotosIndicesToNextLine(CilOperationsMethod cilOperationsMethod)
    {
        List<int> indicesToRemove = [];
        BaseOp[] ops = cilOperationsMethod.Operations;
        for (int index = 0; index < ops.Length - 1; index++)
        {
            BaseOp op = ops[index];
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