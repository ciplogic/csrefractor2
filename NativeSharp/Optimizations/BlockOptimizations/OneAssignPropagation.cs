using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

public class OneAssignPropagation : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        BaseOp[] ops = cilOperationsMethod.Operations;
        for (int i = 1; i < ops.Length; i++)
        {
            BaseOp prev = ops[i - 1];
            if (prev is not LeftOp leftOp)
            {
                continue;
            }

            BaseOp op = ops[i];
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
            cilOperationsMethod.RemoveIndices([i]);
            return true;
        }

        return false;
    }

    private static bool ValidateOp(BaseOp[] ops, int i, string varCode)
    {
        for (int j = 0; j < ops.Length; j++)
        {
            BaseOp op = ops[j];
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
                HashSet<string> usages = new HashSet<string>(InstructionUsages.GetUsagesOf(op));
                if (usages.Contains(varCode))
                {
                    return false;
                }
            }
        }

        return true;
    }
}