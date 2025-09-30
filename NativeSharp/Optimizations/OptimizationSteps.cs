using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.BlockOptimizations;
using NativeSharp.Optimizations.ConstExprOptimization;
using NativeSharp.Optimizations.DeadCodeElimination;
using NativeSharp.Optimizations.GotosOptimizations;
using NativeSharp.Optimizations.Inliner;

namespace NativeSharp.Optimizations;

public class OptimizationSteps(bool isOptimizing)
{
    public OptimizationBase[] CilMethodOptimizations = BuildOptimizations(isOptimizing);

    private static OptimizationBase[] BuildOptimizations(bool isOptimizing)
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
            new RemoveUnusedVars(),
            new InlinerOptimization()
        ];
    }

    public static void OptimizeMethodSet(CilNativeMethod[] methodsToOptimize, OptimizationBase[] cilMethodOptimizations)
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

    private static CilNativeMethod[] CilMethodsFromCache(BaseNativeMethod[] methodCacheValues)
    {
        List<CilNativeMethod> cilMethods = [];
        foreach (var method in methodCacheValues)
        {
            if (method is CilNativeMethod cilNativeMethod)
            {
                cilMethods.Add(cilNativeMethod);
            }
        }

        return cilMethods.ToArray();
    }

    private static void InlineSets(CilNativeMethod[] methodCacheValues)
    {
        var inliner = new InlinerOptimization();
        OptimizeMethodSet(methodCacheValues, [inliner]);
    }

    private static bool OptimizeMethod(CilNativeMethod method, OptimizationBase[] cilMethodOptimizations)
    {
        var result = false;
        foreach (var opt in cilMethodOptimizations)
        {
            result |= opt.Optimize(method);
        }

        return result;
    }

    public static void OptimizeMethodSet(BaseNativeMethod[] methodsToOptimize,
        OptimizationBase[] optimizerCilMethodOptimizations)
    {
        var cilMethods = CilMethodsFromCache(methodsToOptimize);
        OptimizeMethodSet(cilMethods, optimizerCilMethodOptimizations);
      
    }
}