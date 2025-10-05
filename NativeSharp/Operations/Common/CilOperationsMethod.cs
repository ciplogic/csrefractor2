using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Common;

public class CilOperationsMethod : BaseNativeMethod
{
    public BaseOp[] Operations { get; set; } = [];
    public IndexedVariable[] Locals { get; set; } = [];
}