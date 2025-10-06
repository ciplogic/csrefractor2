using NativeSharp.Lib;

namespace NativeSharp.Operations.Common;

public class Cpp(CppNativeContent content) : NativeMethodBase
{
    public CppNativeContent Content { get; } = content; 
}