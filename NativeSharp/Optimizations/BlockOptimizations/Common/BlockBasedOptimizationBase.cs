using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations.Common;

public abstract class BlockBasedOptimizationBase : OptimizationBase
{
    public abstract bool OptimizeSegment(ArraySegment<BaseOp> segment);

    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        ArraySegment<BaseOp>[] basicBlocks = BasicBlocksExtraction(cilOperationsMethod.Operations);
        bool optimized = false;
        foreach (ArraySegment<BaseOp> basicBlock in basicBlocks)
        {
            optimized |= OptimizeSegment(basicBlock);
        }

        return optimized;
    }

    private static ArraySegment<BaseOp>[] BasicBlocksExtraction(BaseOp[] ops)
    {
        List<ArraySegment<BaseOp>> resultList = new (ops.Length / 4);
        int startPos = 0;
        for (int index = 0; index < ops.Length; index++)
        {
            BaseOp op = ops[index];
            if (!op.IsJumpOp())
            {
                continue;
            }

            int len = index - startPos;
            if (len > 0)
            {
                ArraySegment<BaseOp> segment = new ArraySegment<BaseOp>(ops, startPos, len + 1);
                resultList.Add(segment);
            }

            startPos = index + 1;
        }

        int lenFinal = ops.Length - startPos;
        if (lenFinal > 0)
        {
            ArraySegment<BaseOp> segment = new ArraySegment<BaseOp>(ops, startPos, lenFinal);
            resultList.Add(segment);
        }


        return resultList.ToArray();
    }
}