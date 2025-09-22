using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;

namespace NativeSharp.Optimizations.Common;

public static class InstructionExtensions
{
    public static bool IsJumpOp(this BaseOp op) 
        => op switch
        {
            LabelOp or OffsetOp => true,
            _ => false
        };
}