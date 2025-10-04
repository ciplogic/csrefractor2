using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class CallOp : BaseOp
{
    public IValueExpression[] Args { get; set; } = [];
    
    public MethodBase TargetMethod { get; set; } = null!;

    public override BaseOp Clone()
    {
        return new CallOp()
        {
            TargetMethod = TargetMethod,
            Args = Args.ToArray()
        };
    }

    public override string ToString() => $"call {TargetMethod.Name}";

    public override string GenCode()
    {
        string args = string.Join(", ", ArgsCallBuilder.WriteArgsCall(Args, TargetMethod));
        string result = $"{TargetMethod.MangleMethodName()}({args});";
    
        return result;
    }

}