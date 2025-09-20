using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations.Common;

public abstract class BlockBasedOptimizationBase : OptimizationBase
{
    public abstract bool OptimizeSegment(ArraySegment<BaseOp> segment);

    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var basicBlocks = BasicBlocksExtraction(cilNativeMethod.Instructions);
        var optimized = false;
        foreach (var basicBlock in basicBlocks)
        {
            optimized |= OptimizeSegment(basicBlock);
        }

        return optimized;
    }

    private static ArraySegment<BaseOp>[] BasicBlocksExtraction(BaseOp[] ops)
    {
        var resultList = new List<ArraySegment<BaseOp>>(ops.Length / 4);
        var startPos = 0;
        for (var index = 0; index < ops.Length; index++)
        {
            var op = ops[index];
            if (!op.IsJumpOp())
            {
                continue;
            }

            var len = index - startPos;
            if (len > 0)
            {
                var segment = new ArraySegment<BaseOp>(ops, startPos, len + 1);
                resultList.Add(segment);
            }

            startPos = index + 1;
        }

        var lenFinal = ops.Length - startPos;
        if (lenFinal > 0)
        {
            var segment = new ArraySegment<BaseOp>(ops, startPos, lenFinal);
            resultList.Add(segment);
        }


        return resultList.ToArray();
    }
}