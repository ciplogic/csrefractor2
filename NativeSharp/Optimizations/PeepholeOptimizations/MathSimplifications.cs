using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

using static NativeSharp.Optimizations.PeepholeOptimizations.SimplificationUtilities;

namespace NativeSharp.Optimizations.PeepholeOptimizations;

public class MathSimplifications : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        bool result = false;
        BaseOp[] baseOps = cilOperationsMethod.Operations;
        for (int index = 0; index < baseOps.Length; index++)
        {
            result = result || OptimizeOp(baseOps, index);
        }

        return result;
    }

    private static bool OptimizeOp(BaseOp[] baseOps, int index)
    {
        BaseOp op = baseOps[index];
        if (op is BinaryOp binaryOp)
        {
            return OptimizeBinaryOp(baseOps, index, binaryOp);
        }
        return false;
    }

    private static bool OptimizeBinaryOp(BaseOp[] baseOps, int index, BinaryOp binaryOp)
    {
        return binaryOp.Name switch
        {
            "div" => TryOptimizeDivFloat(baseOps, index, binaryOp),
            "cgt" => TryOptimizeCompareGreaterThan(baseOps, index, binaryOp),
            _ => false
        };
    }

    private static bool TryOptimizeCompareGreaterThan(BaseOp[] baseOps, int index, BinaryOp binaryOp)
    {
        if (UpdateBinaryOperation<float>(baseOps, index, (leftValue, rightValue) => leftValue > rightValue ? 1 : 0))
        {
            return true;
        }

        if (UpdateBinaryOperation<double>(baseOps, index, (leftValue, rightValue) => leftValue > rightValue ? 1 : 0))
        {
            return true;
        }
        if (UpdateBinaryOperation<int>(baseOps, index, (leftValue, rightValue) => leftValue > rightValue ? 1 : 0))
        {
            return true;
        }

        return false;
    }

    private static bool TryOptimizeDivFloat(BaseOp[] baseOps, int index, BinaryOp binaryOp)
    {
        IValueExpression rightExpression = binaryOp.RightExpression;
        if (rightExpression is ConstantValueExpression constantValueExpression)
        {
            if (constantValueExpression.ExpressionType == typeof(double))
            {
                double doubleValue = (double)constantValueExpression.Value;
                baseOps[index] = new BinaryOp(binaryOp.Left, "mul")
                {
                    LeftExpression = binaryOp.LeftExpression,
                    RightExpression = new ConstantValueExpression(1.0 / doubleValue),
                };
                return true;
            }
            if (constantValueExpression.ExpressionType == typeof(float))
            {
                float doubleValue = (float)constantValueExpression.Value;
                baseOps[index] = new BinaryOp(binaryOp.Left, "mul")
                {
                    LeftExpression = binaryOp.LeftExpression,
                    RightExpression = new ConstantValueExpression(1.0f / doubleValue),
                };
                return true;
            }
            return false;
        }

        return false;
    }
}