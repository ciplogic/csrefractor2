using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.PeepholeOptimizations;

public class MathSimplifications : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var result = false;
        BaseOp[] baseOps = cilNativeMethod.Instructions;
        for (var index = 0; index < baseOps.Length; index++)
        {
            result = result || OptimizeOp(baseOps, index);
        }

        return result;
    }

    private bool OptimizeOp(BaseOp[] baseOps, int index)
    {
        var op = baseOps[index];
        if (op is BinaryOp binaryOp)
        {
            return OptimizeBinaryOp(baseOps, index, binaryOp);
        }
        return false;
    }

    private bool OptimizeBinaryOp(BaseOp[] baseOps, int index, BinaryOp binaryOp)
    {
        if (binaryOp.Name == "div")
        {
            return TryOptimizeDivFloat(baseOps, index, binaryOp);
        }
        
        return false;
    }

    private static bool TryOptimizeDivFloat(BaseOp[] baseOps, int index, BinaryOp binaryOp)
    {
        var rightExpression = binaryOp.RightExpression;
        if (rightExpression is ConstantValueExpression constantValueExpression)
        {
            if (constantValueExpression.ExpressionType == typeof(double))
            {
                var doubleValue = (double)constantValueExpression.Value;
                baseOps[index] = new BinaryOp(binaryOp.Left)
                {
                    Name = "mul",
                    LeftExpression = binaryOp.LeftExpression,
                    RightExpression = new ConstantValueExpression(1.0 / doubleValue),
                };
                return true;
            }
            if (constantValueExpression.ExpressionType == typeof(float))
            {
                var doubleValue = (float)constantValueExpression.Value;
                baseOps[index] = new BinaryOp(binaryOp.Left)
                {
                    Name = "mul",
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