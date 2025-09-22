namespace NativeSharp.Operations;

internal class LabelOp(int offset) : BaseOp
{
    public int Offset { get; } = offset;

    public override string ToString() => GenCode();

    public override string GenCode()
        => $"label_{Offset}:";
}