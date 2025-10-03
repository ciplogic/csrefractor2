using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

class BinaryOp : LeftOp
{
    public BinaryOp(IndexedVariable left, string name) : base(left)
    {
        Name = name.CleanupFieldName();
    }

    public string Name { get; set; }
    public IValueExpression LeftExpression { get; set; }
    public IValueExpression RightExpression { get; set; }

    public override string GenCode()
        => $"{Left.Code()} = {Name} ({LeftExpression.Code()}, {RightExpression.Code()});";

    public override BaseOp Clone()
    {
        return new BinaryOp(Left, Name)
        {
            LeftExpression = LeftExpression,
            RightExpression = RightExpression
        };
    }
}