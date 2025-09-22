using NativeSharp.Operations.Common;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.DeadCodeElimination;

public class RemoveUnusedVars : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        return cilNativeMethod.RefreshLocalVariables();
    }
}