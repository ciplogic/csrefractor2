using System.Reflection;
using System.Text;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.Inliner;

namespace NativeSharp.Operations.Calls;

public static class ArgsCallBuilder
{

    public static string WriteArgsCall(IValueExpression[] args, MethodBase targetMethod)
    {
        
        var cilMethod = InlinerExtensions.ResolvedMethod(targetMethod);
        if (cilMethod is null)
        {
            return string.Join(", ", args.Select(x => x.Code()));
        }

        var sb = new StringBuilder();
        for (var index = 0; index < args.Length; index++)
        {
            var arg = args[index];
            var callArg = cilMethod.Args[index];
            string argValue = BuildArgCode(arg, callArg);
            sb.Append(argValue);
            if (index < args.Length - 1)
            {
                sb.Append(", ");
            }
        }
        return sb.ToString();
    }

    private static string BuildArgCode(IValueExpression valueExpression, ArgumentVariable callArg)
    {
        if (valueExpression is not Variable v)
        {
            return valueExpression.Code();
        }

        if (v.EscapeResult == callArg.EscapeResult || v.ExpressionType.IsValueType)
        {
            return v.Code();
        }

        return $"{v.Code()}.get()";
    }
}