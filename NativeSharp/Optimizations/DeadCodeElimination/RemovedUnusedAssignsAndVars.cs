using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.DeadCodeElimination;

public class RemovedUnusedAssignsAndVars : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var candidatesForRemoval = new HashSet<string>();

        foreach (BaseOp instruction in cilNativeMethod.Instructions)
        {
            if (instruction is AssignOp assignOp)
            {
                candidatesForRemoval.Add(assignOp.Left.Code());
            }
        }

        foreach (BaseOp instruction in cilNativeMethod.Instructions)
        {
            RemoveUsages(instruction, candidatesForRemoval);
            if (candidatesForRemoval.Count == 0)
            {
                return false;
            }
        }

        var indicesToRemove = GetAssignmentIndicesToRemove(cilNativeMethod, candidatesForRemoval);

        CilMethodExtensions.RemoveIndices(cilNativeMethod, indicesToRemove);
        HashSet<string> usedVariables = GetHashOfUsedVariables(cilNativeMethod);

        var vars = cilNativeMethod.Locals
            .Where(localVar => usedVariables.Contains(localVar.Code()))
            .ToArray();
        cilNativeMethod.Locals = vars;

        return candidatesForRemoval.Count != 0;
    }

    private static int[] GetAssignmentIndicesToRemove(CilNativeMethod cilNativeMethod, HashSet<string> candidatesForRemoval)
    {
        var indicesToRemove = new List<int>();
        for (var index = 0; index < cilNativeMethod.Instructions.Length; index++)
        {
            var instruction = cilNativeMethod.Instructions[index];
            if (instruction is AssignOp assignOp)
            {
                if (candidatesForRemoval.Contains(assignOp.Left.Code()))
                {
                    indicesToRemove.Add(index);
                }
            }
        }

        return indicesToRemove.ToArray();
    }

    private static HashSet<string> GetHashOfUsedVariables(CilNativeMethod cilNativeMethod)
    {
        var usedVariables = new HashSet<string>();
        foreach (var instruction in cilNativeMethod.Instructions)
        {
            var usages = InstructionUsages.GetUsagesOf(instruction);
            foreach (var usage in usages)
            {
                if (!string.IsNullOrEmpty(usage))
                    usedVariables.Add(usage);
            }
        }

        return usedVariables;
    }

    private void RemoveUsages(BaseOp instruction, HashSet<string> candidatesForRemoval)
    {
        IEnumerable<string> usagesOfOp = InstructionUsages.GetUsagesOf(instruction);
        foreach (string usage in usagesOfOp)
        {
            candidatesForRemoval.Remove(usage);
        }
    }

    
}