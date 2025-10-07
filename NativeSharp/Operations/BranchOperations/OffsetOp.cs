namespace NativeSharp.Operations.BranchOperations;

internal abstract class OffsetOp(int offset) : BaseOp
{
    public int Offset { get; set; } = offset;

    public string OffsetStr => CompactNumberWriter.Str(Offset);
}