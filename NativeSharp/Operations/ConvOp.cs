using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

internal class ConvOp(string opName, VReg resultVar, IValueExpression rightSideVar)
    : LeftOp(resultVar)
{
    public string OpName { get; } = opName.Replace('.', '_');
    public IValueExpression RightSideVar { get; } = rightSideVar;

    public override string GenCode()
    {
        return $"{Left.Code()} = {OpName} ({RightSideVar.Code()});";
    }
}