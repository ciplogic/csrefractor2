using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;

namespace NativeSharp.Operations.BranchOperations;

internal class BranchOp : OffsetOp
{
    public string Name { get; }
    public IValueExpression Condition { get; set; }

    public BranchOp(int offset, string name, IValueExpression condition)
        : base(offset)
    {
        Name = name;
        Condition = condition;
    }

    public override string ToString()
        => GenCode();

    public override string GenCode()
        => $"if ({Name.Mangle()}({Condition.Code()})) goto label_{Offset};";
}