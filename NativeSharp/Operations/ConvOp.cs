using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

//TODO: Make it an unary operation 
internal class ConvOp(string opName, IndexedVariable resultVar, IValueExpression rightSideVar)
    : LeftOp(resultVar)
{
    public string OpName { get; } = opName.Replace('.', '_');
    public IValueExpression RightSideVar { get; } = rightSideVar;

    public override string GenCode()
    {
        return $"{Left.Code()} = {OpName} ({RightSideVar.Code()});";
    }

    public override BaseOp Clone()
    {
        return new ConvOp(OpName, Left, RightSideVar);
    }
}