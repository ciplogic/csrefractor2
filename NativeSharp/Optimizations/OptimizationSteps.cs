using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.BlockOptimizations;
using NativeSharp.Optimizations.ConstExprOptimization;
using NativeSharp.Optimizations.DeadCodeElimination;
using NativeSharp.Optimizations.GotosOptimizations;

namespace NativeSharp.Optimizations;

public class OptimizationSteps
{
    public OptimizationBase[] CilMethodOptimizations = BuildOptimizations();

    private static OptimizationBase[] BuildOptimizations(bool enable = true)
    {
        if (!enable)
        {
            return [];
        }

        return
        [
            new BlockBasedPropagation(),
            new RemovedUnusedAssignsAndVars(),
            new GotoOpsOptimizationBase(),
            new HandleConstMethodCalls()
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