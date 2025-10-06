using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class VirtualCallOp : BaseOp, ICallOp, IVirtualCall
{
    public IValueExpression[] Args { get; set; } = [];

    public MethodBase TargetMethod => Resolved.Target;
    public NativeMethodBase Resolved { get; set; } = null!;

    public override BaseOp Clone()
    {
        return new VirtualCallOp()
        {
            Resolved = Resolved,
            Args = Args.ToArray()
        };
    }

    public BaseOp ToStatic() 
        => new CallOp()
        {
            Resolved = Resolved,
            Args = Args
        };

    public override string ToString() => $"virt_call {TargetMethod.Name}";

    public override string GenCode()
    {
        string args = string.Join(", ", ArgsCallBuilder.WriteArgsCall(Args, TargetMethod));
        string result = $"{TargetMethod.MangleMethodName()}({args});";
    
        return result;
    }

}