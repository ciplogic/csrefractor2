using NativeSharp.Operations;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.PeepholeOptimizations;

public static class SimplificationUtilities
{
    
    public static bool UpdateBinaryOperation<T>(BaseOp[] baseOps, int index, Func<T, T, object> mapper)
    {
        BinaryOp? current = baseOps[index] as BinaryOp;
        if (current?.LeftExpression is not ConstantValueExpression leftExpression || leftExpression.ExpressionType != typeof(T))
        {
            return false;
        }

        if (current!.RightExpression is not ConstantValueExpression rightExpression || rightExpression.ExpressionType != typeof(T))
        {
            return false;
        }
        baseOps[index] = new AssignOp(current.Left, new ConstantValueExpression(mapper((T)leftExpression.Value!, (T)rightExpression.Value!)));
        return true;
    }

}