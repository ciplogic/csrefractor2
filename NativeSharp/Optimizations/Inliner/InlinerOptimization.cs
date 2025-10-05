using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.Inliner;

public class InlinerOptimization : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        var ops = cilOperationsMethod.Operations;
        var rowsWithCalls = InlinerExtensions.CalculateIsCandidate(ops);

        if (rowsWithCalls.Length == 0)
        {
            return false;
        }

        var result = false;
        foreach (var row in rowsWithCalls)
        {
            if (ComplexInlinerStep.InlineComplex(cilOperationsMethod, row))
            {
                result = true;
            }
        }

        return result;
    }
}