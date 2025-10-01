using System.Reflection;
using NativeSharp.Lib.Resolvers;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.ConstExprOptimization;

public class HandleConstMethodCalls : OptimizationBase
{
    public override bool Optimize(CilNativeMethod cilNativeMethod)
    {
        var result = false;
        for (var index = 0; index < cilNativeMethod.Instructions.Length; index++)
        {
            BaseOp[] ops = cilNativeMethod.Instructions;
            var instruction = ops[index];
            if (instruction is not CallReturnOp callReturnOp)
            {
                continue;
            }

            if (!MethodIsOptimizable(callReturnOp.TargetMethod, callReturnOp.Args))
            {
                continue;
            }

            var arguments = ConstArguments(callReturnOp.Args);
            var resultCall = callReturnOp.TargetMethod.Invoke(null, arguments)!;

            var constantResult = new ConstantValueExpression(resultCall);
            ops[index] = new AssignOp(left: callReturnOp.Left, constantResult);
            result = true;
        }

        return result;
    }

    private bool MethodIsOptimizable(MethodBase targetMethod, IValueExpression[] args)
    {
        if (!AreMethodParameterConstants(args))
        {
            return false;
        }
        var pureMethod = targetMethod.GetCustomAttributes<PureMethodAttribute>()
            .FirstOrDefault();
        return pureMethod is not null;
    }

    private static bool AreMethodParameterConstants(IValueExpression[] args)
    {
        foreach (var arg in args)
        {
            if (arg is not ConstantValueExpression)
            {
                return false;
            }
        }

        return true;
    }

    private static object[] ConstArguments(IValueExpression[] args)
        => args
            .Select(x => (ConstantValueExpression)x)
            .Select(x => x.Value)
            .ToArray();
}