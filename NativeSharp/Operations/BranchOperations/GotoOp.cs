namespace NativeSharp.Operations.BranchOperations;

internal class GotoOp (int offset) : JumpToOffset(offset)
{
    public override string GenCode() 
        => $"goto label_{OffsetStr};";

    public override BaseOp Clone() => new GotoOp(Offset);
}