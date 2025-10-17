using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.GotosOptimizations;

public class DataflowUnreachableCodeDeleter : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        int[] visitedRows = VisitedOps(cilOperationsMethod);
        if (visitedRows.Length == cilOperationsMethod.Operations.Length)
        {
            return false;
        }

        int[] unusedRows = CalculateUnusedRows(visitedRows, cilOperationsMethod.Operations.Length);
        cilOperationsMethod.RemoveIndices(unusedRows);
        return true;
    }

    private static int[] VisitedOps(CilOperationsMethod cilOperationsMethod)
    {
        HashSet<int> foundRows = [];
        var operations = cilOperationsMethod.Operations;
        List<int> nextToVisit = [0];
        while (nextToVisit.Count > 0)
        {
            var rowToVisit = nextToVisit[^1];
            nextToVisit.RemoveAt(nextToVisit.Count-1);
            VisitBlock(foundRows, operations, rowToVisit, nextToVisit);
        }
        
        return foundRows.ToArray();
    }

    private static void VisitBlock(HashSet<int> foundRows, BaseOp[] operations, int startRow, List<int> nextToVisit)
    {
        while (startRow < operations.Length)
        {
            foundRows.Add(startRow);
            var currentOp = operations[startRow++];
            if (currentOp is GotoOp gotoOp)
            {
                AddNextToVisit(gotoOp.Offset);
                return;
            }

            if (currentOp is BranchOp branchOp)
            {
                AddNextToVisit( branchOp.Offset);
            }
        }

        return;

        void AddNextToVisit(int offset)
        {
            if (!foundRows.Contains(offset))
            {
                nextToVisit.Add(IndexOfLabel(operations, offset));
            }
        }
    }

    private static int IndexOfLabel(BaseOp[] ops, int offset)
    {
        for (var index = 0; index < ops.Length; index++)
        {
            var op = ops[index];
            if (op is LabelOp labelOp)
            {
                if (labelOp.Offset == offset)
                {
                    return index;
                }
            }
        }
        return -1;
    }

    private static int[] CalculateUnusedRows(int[] visitedRows, int operationsLength)
    {
        var hash = new HashSet<int>(visitedRows);
        var itemsToRemove = Enumerable.Range(0, operationsLength)
            .Where(it => !hash.Contains(it))
            .ToArray();
        return itemsToRemove;
    }
}