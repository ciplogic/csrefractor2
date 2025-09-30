using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

public class OneAssignPropagation : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var ops = cilNativeMethod.Instructions;
        for (var i = 1; i < ops.Length; i++)
        {
            var prev = ops[i - 1];
            if (prev is not LeftOp leftOp)
            {
                continue;
            }

            var op = ops[i];
            if (op is not AssignOp assignOp)
            {
                continue;
            }

            if (leftOp.Left.Code() != assignOp.Expression.Code())
            {
                continue;
            }

            if (!ValidateOp(ops, i, leftOp.Left.Code()))
            {
                continue;
            }
            leftOp.Left = assignOp.Left;
            CilNativeMethodExtensions.RemoveIndices(cilNativeMethod, [i]);
            return true;
        }

        return false;
    }

    private static bool ValidateOp(BaseOp[] ops, int i, string varCode)
    {
        for (var j = 0; j < ops.Length; j++)
        {
            var op = ops[j];
            if (op is LeftOp leftOp)
            {
                if (j == i - 1)
                {
                    continue;
                }

                if (leftOp.Left.Code() == varCode)
                {
                    return false;
                }
            }

            if (j != i)
            {
                var usages = new HashSet<string>(InstructionUsages.GetUsagesOf(op));
                if (usages.Contains(varCode))
                {
                    return false;
                }
            }
        }

        return true;
    }
}