using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations;

public abstract class OptimizationBase
{
    public abstract bool Optimize(CilNativeMethod cilNativeMethod);
}