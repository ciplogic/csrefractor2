using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public class CallOp : BaseOp, ICallOp
{
    public IValueExpression[] Args { get; set; } = [];

    public MethodBase TargetMethod => Resolved.Target;
    public NativeMethodBase Resolved { get; set; }

    public override BaseOp Clone()
    {
        return new CallOp()
        {
            Resolved = Resolved,
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

public interface IVirtualCall
{
    BaseOp ToStatic();
}