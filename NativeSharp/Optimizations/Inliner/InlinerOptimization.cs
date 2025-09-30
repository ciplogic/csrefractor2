using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.Inliner;

public class InlinerOptimization : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var ops = cilNativeMethod.Instructions;
        var rowsWithCalls = InlinerExtensions.CalculateIsCandidate(ops);

        if (rowsWithCalls.Length == 0)
        {
            return false;
        }

        var result = false;
        foreach (var row in rowsWithCalls)
        {
            if (ComplexInlinerStep.InlineComplex(cilNativeMethod, row))
            {
                result = true;
            }
        }

        return result;
    }
}