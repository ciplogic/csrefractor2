using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations;

public class CallOp : BaseOp
{
    public CallType CallType { get; set; }
    public IValueExpression[] Args { get; set; } = [];
    
    public MethodBase TargetMethod { get; set; } = null!;

    public override BaseOp Clone()
    {
        return new CallOp()
        {
            TargetMethod = TargetMethod,
            CallType = CallType,
            Args = Args.ToArray()
        };
    }

    public override string ToString() => $"call {TargetMethod.Name}";

    public override string GenCode()
    {
        string args = string.Join(", ", Args.Select(x => x.Code()));
        string result = $"{TargetMethod.MangleMethodName()}({args});";
    
        return result;
    }
}

public enum CallType
{
    Virtual,
    Static,
    Native,
    Dynamic,
}