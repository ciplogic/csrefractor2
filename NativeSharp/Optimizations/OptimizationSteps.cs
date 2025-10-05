using NativeSharp.Extensions;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.BlockOptimizations;
using NativeSharp.Optimizations.ConstExprOptimization;
using NativeSharp.Optimizations.DeadCodeElimination;
using NativeSharp.Optimizations.GotosOptimizations;
using NativeSharp.Optimizations.Inliner;
using NativeSharp.Optimizations.PeepholeOptimizations;

namespace NativeSharp.Optimizations;

public class OptimizationSteps(bool isOptimizing)
{
    public List<OptimizationBase> CilMethodOptimizations = BuildOptimizations(isOptimizing);

    private static List<OptimizationBase> BuildOptimizations(bool isOptimizing)
    {
        if (!isOptimizing)
        {
            return [];
        }

        return
        [
            new BlockBasedPropagation(),
            new RemovedUnusedAssignsAndVars(),
            new GotoOpsOptimization(),
            new HandleConstMethodCalls(),
            new OneAssignPropagation(),
            new MathSimplifications(),
            new RemoveUnusedVars(),
            new InlinerOptimization()
        ];
    }

    public static void OptimizeMethodSet(CilOperationsMethod[] methodsToOptimize, OptimizationBase[] cilMethodOptimizations)
    {
        bool canOptimize;
        do
        {
            canOptimize = false;
            foreach (var cilNativeMethod in methodsToOptimize)
            {
                canOptimize |= OptimizeMethod(cilNativeMethod, cilMethodOptimizations);
            }
        } while (canOptimize);
    }

    private static bool OptimizeMethod(CilOperationsMethod method, OptimizationBase[] cilMethodOptimizations)
    {
        var result = false;
        foreach (var opt in cilMethodOptimizations)
        {
            result |= opt.Optimize(method);
        }

        return result;
    }

    public static void OptimizeMethodSet(OptimizationBase[] optimizerCilMethodOptimizations)
    {
        var cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        OptimizeMethodSet(cilMethods, optimizerCilMethodOptimizations);
      
    }
}