using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd.Transformers;

internal class LoadNullOp(VReg vReg) : LeftOp(vReg)
{
    public override string GenCode()
    {
        //Should be simplified later
        throw new NotImplementedException();
    }
}