namespace NativeSharp.Operations.BranchOperations;

internal abstract class OffsetOp(int offset) : BaseOp
{
    public int Offset { get; set; } = offset;
}