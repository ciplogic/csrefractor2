using System.Reflection;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Calls;

public interface ICallOp
{
    IValueExpression[] Args { get; }
    public MethodBase TargetMethod { get; }
    public NativeMethodBase Resolved { get; set; }
}