using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.BlockOptimizations;
using NativeSharp.Optimizations.ConstExprOptimization;
using NativeSharp.Optimizations.DeadCodeElimination;
using NativeSharp.Optimizations.GotosOptimizations;
using NativeSharp.Optimizations.Inliner;
using NativeSharp.Optimizations.PeepholeOptimizations;

namespace NativeSharp.Optimizations;

public class OptimizationSteps(OptimizationOptions isOptimizing)
{
    public OptimizationBase[] CilMethodOptimizations = BuildOptimizations(isOptimizing).ToArray();

    private static OptimizationBase[] BuildOptimizations(OptimizationOptions optimizationOptions)
    {

        List<OptimizationBase> optimizationList =
        [
            new BlockBasedPropagation(),
            new BlockBasedFieldSetterRemoval(),

            new RemovedUnusedAssignsAndVars(),
            new RemoveUnusedVars(),
            new GotoOpsOptimization(),
            new HandleConstMethodCalls(),
            new OneAssignPropagation(),
            new MathSimplifications(),
            new BranchIfConstantsOptimizations(),
            new DataflowUnreachableCodeDeleter(),
        ];
        if (optimizationOptions.UseInlining)
        {
            optimizationList.Add(new InlinerOptimization());
        }

        if (optimizationOptions.UseFieldDeduplication)
        {
            optimizationList.Add(new BlockBasedDededuplicatedFieldReads());
            optimizationList.Add(new BlockBasedDededuplicatedArrayReads());
        }

        return optimizationList.ToArray();
    }

    public static void OptimizeMethodSet(CilOperationsMethod[] methodsToOptimize,
        OptimizationBase[] cilMethodOptimizations)
    {
        bool canOptimize;
        do
        {
            canOptimize = false;
            foreach (CilOperationsMethod cilNativeMethod in methodsToOptimize)
            {
                canOptimize |= OptimizeMethod(cilNativeMethod, cilMethodOptimizations);
            }
        } while (canOptimize);
    }

    private static bool OptimizeMethod(CilOperationsMethod method, OptimizationBase[] cilMethodOptimizations)
    {
        bool result = false;
        foreach (OptimizationBase opt in cilMethodOptimizations)
        {
            result |= opt.Optimize(method);
        }

        return result;
    }

    public static void OptimizeMethodSet(OptimizationBase[] optimizerCilMethodOptimizations)
    {
        CilOperationsMethod[] cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        OptimizeMethodSet(cilMethods, optimizerCilMethodOptimizations);
    }
}