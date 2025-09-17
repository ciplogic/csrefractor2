using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.BlockOptimizations;
using NativeSharp.Optimizations.DeadCodeElimination;
using NativeSharp.Optimizations.GotosOptimizations;

namespace NativeSharp.Optimizations;

public class OptimizationSteps
{
    public CilMethodOptimization[] CilMethodOptimizations = BuildOptimizations();

    private static CilMethodOptimization[] BuildOptimizations(bool enable = true)
    {
        if (!enable)
        {
            return [];
        }

        return
        [
            new BlockBasedPropagation(),
            new RemovedUnusedAssignsAndVars(),
            new GotoOpsOptimization()
        ];
    }

    public bool OptimizeMethod(CilNativeMethod method)
    {
        bool result = false;
        while (OptimizeMethodOptimizations(method))
        {
            result = true;
        }

        return result;
    }

    public bool OptimizeMethodOptimizations(CilNativeMethod method)
    {
        var result = false;
        foreach (var opt in CilMethodOptimizations)
        {
            result |= opt.Optimize(method);
        }

        return result;
    }
}