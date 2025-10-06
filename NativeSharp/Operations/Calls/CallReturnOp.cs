using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class CallReturnOp(IndexedVariable left) 
    : LeftOp(left), ICallOp
{
    public IValueExpression[] Args { get; set; } = [];

    public MethodBase TargetMethod => Resolved.Target;
    public NativeMethodBase Resolved { get; set; }

    public override BaseOp Clone() =>
        new CallReturnOp(Left)
        {
            Args = Args.ToArray(),
            Resolved = Resolved,
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