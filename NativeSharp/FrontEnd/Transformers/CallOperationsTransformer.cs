using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.Common;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd.Transformers;

internal static class CallOperationsTransformer
{
    public static readonly Type[] EmptyConstructorTypes = [typeof(object)];

    public static BaseOp TransformCallOp(LocalVariablesStackAndState locals, Instruction instruction)
    {
        var opName = instruction.OpCode.Name!;
        MethodBase operand = (MethodBase)instruction.Operand;
        bool isCallVirtual = opName == "callvirt";
        if (EmptyConstructorTypes.Contains(operand.DeclaringType))
        {
            locals.Pop();
            return new CompositeOp([]);
        }

        MethodInfo? operandAsMethodInfo = operand as MethodInfo;

        int paramCount = operandAsMethodInfo?.GetParameters().Length ?? 0;

        var argumentArray = BuildArgumentArray(locals, paramCount, operandAsMethodInfo);

        Type returnType = operandAsMethodInfo?.ReturnType ?? typeof(void);
        VReg? returnValue = null;

        MethodResolver.ResolveMethod(operand);
        if (returnType != typeof(void))
        {
            returnValue = locals.NewVirtVar(returnType);

            return isCallVirtual
                ? new CallVirtualReturnOp(returnValue)
                {
                    TargetMethod = operand,
                    Args = argumentArray
                }
                : new CallReturnOp(returnValue)
                {
                    TargetMethod = operand,
                    Args = argumentArray
                };
        }

        return isCallVirtual
            ? new CallVirtualOp()
            {
                TargetMethod = operand,
                Args = argumentArray
            }
            : new CallOp()
            {
                TargetMethod = operand,
                Args = argumentArray
            };
    }

    private static IValueExpression[] BuildArgumentArray(LocalVariablesStackAndState locals, int paramCount,
        MethodInfo? operandAsMethodInfo)
    {
        List<IValueExpression> argumentList = new List<IValueExpression>();
        for (int i = 0; i < paramCount; i++)
        {
            argumentList.Add(locals.Pop());
        }

        if (operandAsMethodInfo != null && !operandAsMethodInfo.IsStatic)
        {
            //makes sure that this pointer is also pushed for non static methods.
            argumentList.Add(locals.Pop());
        }

        argumentList.Reverse();
        
        IValueExpression[] argumentArray = argumentList.ToArray();
        return argumentArray;
    }
}