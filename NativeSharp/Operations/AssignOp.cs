using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

class AssignOp : LeftOp
{
    public IValueExpression Expression { get; set; }

    public AssignOp(IndexedVariable left, IValueExpression expression) : base(left)
    {
        Expression = expression;
    }

    public override string GenCode()
        => $"{Left.Code()} = {Expression.Code()};";

    public override BaseOp Clone() 
        => new AssignOp(Left, Expression);
}