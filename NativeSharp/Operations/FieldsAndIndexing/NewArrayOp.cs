using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.FieldsAndIndexing;

internal class NewArrayOp : LeftOp
{
    public Type ElementType { get; set; }
    public IValueExpression Count { get; set; }

    public NewArrayOp(IndexedVariable result, Type elementType, IValueExpression count)
        : base(result)
    {
        ElementType = elementType;
        Count = count;
    }

    public override string GenCode()
        => $"{Left.Code()} = new_arr<{ElementType.Mangle()}>({Count.Code()});";

    public override BaseOp Clone() => new NewArrayOp(Left, ElementType, Count);
}