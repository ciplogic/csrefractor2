using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class CallReturnOp(IndexedVariable left, CallType callType) 
    : LeftOp(left)
{
    public CallType CallType { get; set; } = callType;
    public IValueExpression[] Args { get; set; } = [];
    public IValueExpression? ReturnValue { get; set; } = null;

    public MethodBase TargetMethod { get; set; } = null!;

    public override BaseOp Clone() =>
        new CallReturnOp(Left, CallType)
        {
            Args = Args.ToArray(),
            ReturnValue = ReturnValue,
            TargetMethod = TargetMethod
        };

    public override string ToString() => $"call {TargetMethod.Name}";

    public override string GenCode()
    {
        string args = string.Join(", ", ArgsCallBuilder.WriteArgsCall(Args, TargetMethod));
        string result = $"{TargetMethod.MangleMethodName()}({args});";
        result = $"{Left.Code()} = {result}";

        return result;
    }
}