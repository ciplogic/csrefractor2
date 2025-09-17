using NativeSharp.Operations;

namespace NativeSharp.Optimizations.BlockOptimizations;

public static class InstructionExtensions
{
    public static bool IsJumpOp(this BaseOp op)
    {
        switch (op)
        {
            case LabelOp:
            case BranchOp:
            case GotoOp:
                return true;
            default:
                return false;
        }
    }
}