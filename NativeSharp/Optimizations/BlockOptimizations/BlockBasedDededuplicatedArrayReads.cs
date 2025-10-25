using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Optimizations.BlockOptimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

public class BlockBasedDededuplicatedArrayReads : BlockBasedOptimizationBase
{
    public override bool OptimizeSegment(ArraySegment<BaseOp> segment)
    {

        bool containsArrayElementRead = segment.AsSpan().Contains<LoadElementOp>();
        if (!containsArrayElementRead)
        {
            return false;
        }

        return false;
    }
}