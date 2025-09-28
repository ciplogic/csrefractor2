namespace NativeSharp.Operations;

public abstract class BaseOp
{
    public abstract string GenCode();
    public abstract BaseOp Clone();

    public override string ToString() => GenCode();
}