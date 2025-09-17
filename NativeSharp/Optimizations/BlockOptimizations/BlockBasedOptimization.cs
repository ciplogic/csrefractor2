using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

public abstract class BlockBasedOptimization : CilMethodOptimization
{
    public abstract bool OptimizeSegment(ArraySegment<BaseOp> segment);

    private static ArraySegment<BaseOp>[] BasicBlocksExtraction(BaseOp[] ops)
    {
        var resultList = new List<ArraySegment<BaseOp>>();
        var startPos = 0;
        for (var index = 0; index < ops.Length; index++)
        {
            var op = ops[index];
            if (op.IsJumpOp())
            {
                var len = index - startPos;
                if (len > 0)
                {
                    var segment = new ArraySegment<BaseOp>(ops, startPos, len+1);
                    resultList.Add(segment);
                }

                startPos = index + 1;
            }
        }

        var lenFinal = ops.Length - startPos;
        if (lenFinal > 0)
        {
            var segment = new ArraySegment<BaseOp>(ops, startPos, lenFinal);
            resultList.Add(segment);
        }


        return resultList.ToArray();
    }

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
}