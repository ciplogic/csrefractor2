using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class VirtualCallReturnOp(IndexedVariable left) 
    : LeftOp(left), ICallOp, IVirtualCall
{
    public IValueExpression[] Args { get; set; } = [];

    public MethodBase TargetMethod { get; set; } = null!;

    public override BaseOp Clone() =>
        new VirtualCallReturnOp(Left)
        {
            Args = Args.ToArray(),
            TargetMethod = TargetMethod
        };

    public BaseOp ToStatic()
        => new CallReturnOp(Left)
        {
            Args = Args,
            TargetMethod = TargetMethod
        };

    public override string ToString() => $"virt_call {TargetMethod.MangleMethodName()}";

    public override string GenCode()
    {
        string args = string.Join(", ", ArgsCallBuilder.WriteArgsCall(Args, TargetMethod));
        string result = $"{TargetMethod.MangleMethodName()}({args});";
        result = $"{Left.Code()} = {result}";

        return result;
    }
}