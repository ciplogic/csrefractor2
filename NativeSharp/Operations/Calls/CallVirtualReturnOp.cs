using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class CallVirtualReturnOp(IndexedVariable left) 
    : LeftOp(left)
{
    public IValueExpression[] Args { get; set; } = [];

    public MethodBase TargetMethod { get; set; } = null!;

    public override BaseOp Clone() =>
        new CallVirtualReturnOp(Left)
        {
            Args = Args.ToArray(),
            TargetMethod = TargetMethod
        };

    public override string ToString() => $"call {TargetMethod.MangleMethodName()}";

    public override string GenCode()
    {
        string args = string.Join(", ", ArgsCallBuilder.WriteArgsCall(Args, TargetMethod));
        string result = $"{TargetMethod.MangleMethodName()}({args});";
        result = $"{Left.Code()} = {result}";

        return result;
    }
}