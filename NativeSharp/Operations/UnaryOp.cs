using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

class UnaryOp : LeftOp
{
    public UnaryOp(IndexedVariable left) : base(left)
    {
    }

    public string Operator { get; set; } = null!;
    public IValueExpression LeftExpression { get; set; }

    public override string GenCode()
        => $"{Left.Code()} = {Operator} ({LeftExpression.Code()});";
}