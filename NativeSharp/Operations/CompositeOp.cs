namespace NativeSharp.Operations;

class CompositeOp(BaseOp[] ops) : BaseOp
{
    public BaseOp[] Ops { get; } = ops;

    public override string GenCode()
    {
        return string.Join("\n", Ops.Select(op => op.GenCode()));
    }

    public override BaseOp Clone()
    {
        return new CompositeOp(Ops.ToArray());
    }
}