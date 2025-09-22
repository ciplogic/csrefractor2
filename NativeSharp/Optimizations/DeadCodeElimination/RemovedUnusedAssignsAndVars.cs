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

        HashSet<string> usages = cilNativeMethod.Instructions.GetUsagesOfOps();

        foreach (var usage in usages)
        {
            candidatesForRemoval.Remove(usage);
        }

        if (candidatesForRemoval.Count == 0)
        {
            return false;
        }


        var indicesToRemove = GetAssignmentIndicesToRemove(cilNativeMethod, candidatesForRemoval);

        CilMethodExtensions.RemoveIndices(cilNativeMethod, indicesToRemove); 
        
        return candidatesForRemoval.Count != 0;
    }

    private static int[] GetAssignmentIndicesToRemove(CilNativeMethod cilNativeMethod,
        HashSet<string> candidatesForRemoval)
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
            var usages = InstructionUsages.GetUsagesOfArr(instruction);
            foreach (var usage in usages)
            {
                usedVariables.Add(usage);
            }
        }

        return usedVariables;
    }

    private void RemoveUsages(BaseOp instruction, HashSet<string> candidatesForRemoval)
    {
        string[] usagesOfOp = InstructionUsages.GetUsagesOfArr(instruction);
        foreach (string usage in usagesOfOp)
        {
            candidatesForRemoval.Remove(usage);
        }
    }
}