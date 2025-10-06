using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.Inliner;

public class InlinerOptimization : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        BaseOp[] ops = cilOperationsMethod.Operations;
        int[] rowsWithCalls = InlinerExtensions.CalculateIsCandidate(ops);

        if (rowsWithCalls.Length == 0)
        {
            return false;
        }

        bool result = false;
        foreach (int row in rowsWithCalls)
        {
            if (ComplexInlinerStep.InlineComplex(cilOperationsMethod, row))
            {
                result = true;
            }
        }

        return result;
    }
}