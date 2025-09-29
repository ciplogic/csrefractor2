using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

internal class LoadNullOp(IndexedVariable vReg) : LeftOp(vReg)
{
    public override string GenCode()
    {
        //Should be simplified later
        throw new NotImplementedException();
    }

    public override BaseOp Clone() => new LoadNullOp(Left);
}