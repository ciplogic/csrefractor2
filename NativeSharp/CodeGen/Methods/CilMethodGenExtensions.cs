using NativeSharp.EscapeAnalysis;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.CodeGen.Methods;

public static class CilMethodGenExtensions
{
    public static string MangledMethodHeader(this NativeMethodBase cil)
    {
        string args = string.Join(", ",
            cil.Args.Select(x => MangleArg(x)));
        string methodHeader =
            $"{cil.Target.MangleMethodReturnType()} {cil.Target.MangleMethodName()}({args})";
        return methodHeader;
    }

    static string MangleArg(ArgumentVariable x)
    {
        if (x.ExpressionType.IsValueType || x.EscapeResult == EscapeKind.Local)
        {
            return $"{x.ExpressionType.Mangle(x.EscapeResult)} {x.GenCodeImpl()}";
        }

        return $"{x.ExpressionType.Mangle(x.EscapeResult)}& {x.GenCodeImpl()}";
    }
}