using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.Common;
using NativeSharp.Optimizations.DeadCodeElimination;

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

        foreach (var row in rowsWithCalls)
        {
            if (TryInline(cilNativeMethod, row))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryInline(CilNativeMethod cilNativeMethod, int row)
    {
        var complexInliner = new ComplexInlinerStep();
        return ComplexInlinerStep.InlineComplex(cilNativeMethod, row);
    }

    private bool TryInlineCallOp(CilNativeMethod cilNativeMethod, int row, CallOp callOp)
    {
        var targetToInline = InlinerExtensions.ResolvedMethod(callOp.TargetMethod)!;
        var targetOps = targetToInline.Instructions;

        if (targetOps.Length == 1)
        {
            //this is empty method or discarded getter
            CilMethodExtensions.RemoveIndices(cilNativeMethod, [row]);
            return true;
        }

        if (targetOps.Length == 2)
        {
            var clonedOp = targetToInline.Instructions[0].Clone();
            Dictionary<IValueExpression, IValueExpression> fromTo = new();
            for (var i = 0; i < callOp.Args.Length; i++)
            {
                fromTo[targetToInline.Args[i]] = callOp.Args[i];
            }

            InstructionUsages.UpdateKnownOpUsages(clonedOp, fromTo);
            cilNativeMethod.Instructions[row] = clonedOp;

            return true;
        }

        return TryInlineComplexCall(cilNativeMethod, row, callOp, targetToInline);

        return false;
    }

    private bool TryInlineComplexCall(CilNativeMethod cilNativeMethod, int row, CallOp callOp,
        CilNativeMethod targetToInline)
    {
        var targetOps = targetToInline.Instructions;
        if (targetOps.Length > 120)
        {
            return false;
        }

        Dictionary<IValueExpression, IValueExpression> mappedFromInline =
            new Dictionary<IValueExpression, IValueExpression>();


        return false;
    }
}