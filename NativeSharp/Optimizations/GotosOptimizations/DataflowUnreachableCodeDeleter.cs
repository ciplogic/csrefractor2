using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.GotosOptimizations;

public class DataflowUnreachableCodeDeleter : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        if (!cilOperationsMethod.Contains<LabelOp>())
        {
            return false;
        }
        
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
        SortedSet<int> foundRows = [];
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

    private static void VisitBlock(SortedSet<int> foundRows, BaseOp[] operations, int startRow, List<int> nextToVisit)
    {
        while (startRow < operations.Length)
        {
            if (foundRows.Count == operations.Length)
            {
                return;
            }
            foundRows.Add(startRow);
            var currentOp = operations[startRow];

            startRow++;
            if (currentOp is not JumpToOffset jumpToOffset)
            {
                continue;
            }

            int labelIndex = IndexOfLabel(operations, jumpToOffset.Offset);
            AddNextToVisit(labelIndex, foundRows, nextToVisit);
            if (currentOp is GotoOp)
            {
                return;
            }
        }
    }

    private static void AddNextToVisit(int indexOfLabel, SortedSet<int> foundRows, List<int> toVisit)
    {
        if (!foundRows.Contains(indexOfLabel))
        {
            toVisit.Add(indexOfLabel);
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