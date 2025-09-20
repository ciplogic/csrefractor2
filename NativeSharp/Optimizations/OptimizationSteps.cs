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

    public void OptimizeMethodSet(BaseNativeMethod[] methodCacheValues)
    {
        bool canOptimize;
        do
        {
            canOptimize = false;
            foreach (var method in methodCacheValues)
            {
                if (method is CilNativeMethod cilNativeMethod)
                {
                    canOptimize |= OptimizeMethod(cilNativeMethod);
                }
            }
        } while (canOptimize);
    }

    private bool OptimizeMethod(CilNativeMethod method)
    {
        var result = false;
        foreach (var opt in CilMethodOptimizations)
        {
            result |= opt.Optimize(method);
        }

        return result;
    }
}