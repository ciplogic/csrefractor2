using NativeSharp.Operations;

namespace NativeSharp.Optimizations.Common;

public static class InstructionExtensions
{
    public static bool IsJumpOp(this BaseOp op) 
        => op switch
        {
            LabelOp or BranchOp or GotoOp => true,
            _ => false
        };
}