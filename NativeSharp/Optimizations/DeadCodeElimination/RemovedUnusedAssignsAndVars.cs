using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.DeadCodeElimination;

public class RemovedUnusedAssignsAndVars : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        HashSet<string> candidatesForRemoval = new HashSet<string>();

        foreach (BaseOp instruction in cilOperationsMethod.Operations)
        {
            if (instruction is AssignOp assignOp)
            {
                candidatesForRemoval.Add(assignOp.Left.Code());
            }
        }

        HashSet<string> usages = cilOperationsMethod.Operations.GetUsagesOfOps();

        foreach (string usage in usages)
        {
            candidatesForRemoval.Remove(usage);
        }

        if (candidatesForRemoval.Count == 0)
        {
            return false;
        }


        int[] indicesToRemove = GetAssignmentIndicesToRemove(cilOperationsMethod, candidatesForRemoval);

        CilNativeMethodExtensions.RemoveIndices(cilOperationsMethod, indicesToRemove); 
        
        return candidatesForRemoval.Count != 0;
    }

    private static int[] GetAssignmentIndicesToRemove(CilOperationsMethod cilOperationsMethod,
        HashSet<string> candidatesForRemoval)
    {
        List<int> indicesToRemove = new List<int>();
        for (int index = 0; index < cilOperationsMethod.Operations.Length; index++)
        {
            BaseOp instruction = cilOperationsMethod.Operations[index];
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

    private static HashSet<string> GetHashOfUsedVariables(CilOperationsMethod cilOperationsMethod)
    {
        HashSet<string> usedVariables = new HashSet<string>();
        foreach (BaseOp instruction in cilOperationsMethod.Operations)
        {
            string[] usages = InstructionUsages.GetUsagesOfArr(instruction);
            foreach (string usage in usages)
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