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
    public OptimizationBase[] CilMethodOptimizations = BuildOptimizations(isOptimizing).ToArray();

    private static List<OptimizationBase> BuildOptimizations(bool isOptimizing)
    {
        if (!isOptimizing)
        {
            return [];
        }

        return
        [
            new BlockBasedPropagation(),
            new BlockBasedFieldSetterRemoval(),
            new RemovedUnusedAssignsAndVars(),
            new GotoOpsOptimization(),
            new HandleConstMethodCalls(),
            new OneAssignPropagation(),
            new MathSimplifications(),
            new BranchIfConstantsOptimizations(),
            new DataflowUnreachableCodeDeleter(),
            
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