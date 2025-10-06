using NativeSharp.Operations;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.BlockOptimizations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

internal class BlockBasedPropagation : BlockBasedOptimizationBase
{
    public override bool OptimizeSegment(ArraySegment<BaseOp> segment)
    {
        Dictionary<IValueExpression, IValueExpression> updates = [];

        bool result = false;
        for (int i = 0; i < segment.Count; i++)
        {
            BaseOp op = segment[i];
            result |= InstructionUsages.UpdateKnownOpUsages(op, updates);
            if (op is not AssignOp assignOp)
            {
                continue;
            }

            if (assignOp.Expression is ConstantValueExpression || assignOp.Expression is IndexedVariable)
            {
                updates[assignOp.Left] = assignOp.Expression;
            }
        }

        return result;
    }

}