using System.Reflection;
using NativeSharp.Common;
using NativeSharp.Lib.Resolvers;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.ConstExprOptimization;

public class HandleConstMethodCalls : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        bool result = false;
        for (int index = 0; index < cilOperationsMethod.Operations.Length; index++)
        {
            BaseOp[] ops = cilOperationsMethod.Operations;
            BaseOp instruction = ops[index];
            if (instruction is not CallReturnOp callReturnOp)
            {
                continue;
            }

            if (!MethodIsOptimizable(callReturnOp.TargetMethod, callReturnOp.Args))
            {
                continue;
            }

            object[] arguments = ConstArguments(callReturnOp.Args);
            object resultCall = callReturnOp.TargetMethod.Invoke(null, arguments)!;

            ConstantValueExpression constantResult = new ConstantValueExpression(resultCall);
            ops[index] = new AssignOp(left: callReturnOp.Left, constantResult);
            result = true;
        }

        return result;
    }

    private static bool MethodIsOptimizable(MethodBase targetMethod, IValueExpression[] args)
    {
        if (!AreMethodParameterConstants(args))
        {
            return false;
        }

        PureMethodAttribute? pureMethod = targetMethod
            .GetCustomAttributes<PureMethodAttribute>()
            .FirstOrDefault();
        return pureMethod is not null;
    }

    private static bool AreMethodParameterConstants(IValueExpression[] args)
    {
        foreach (IValueExpression arg in args)
        {
            if (arg is not ConstantValueExpression)
            {
                return false;
            }
        }

        return true;
    }

    private static object[] ConstArguments(IValueExpression[] args)
        => args.SelectToArray(x => ((ConstantValueExpression)x).Value);
}