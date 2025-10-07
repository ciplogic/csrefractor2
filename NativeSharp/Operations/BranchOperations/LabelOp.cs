namespace NativeSharp.Operations.BranchOperations;

internal class LabelOp(int offset) : OffsetOp(offset)
{
    public override BaseOp Clone() => new LabelOp(Offset);

    public override string ToString() => GenCode();

    public override string GenCode()
        => $"label_{OffsetStr}:";
}