using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.Common;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd.Transformers;

internal static class CallOperationsTransformer
{
    public static readonly Type[] EmptyConstructorTypes = [typeof(object)];

    public static BaseOp TransformCallOp(LocalVariablesStackAndState locals, Instruction instruction)
    {
        var opName = instruction.OpCode.Name!;
        var operand = (MethodBase)instruction.Operand;
        var isCallVirtual = opName == "callvirt";
        if (EmptyConstructorTypes.Contains(operand.DeclaringType))
        {
            locals.Pop();
            return new CompositeOp([]);
        }

        var operandAsMethodInfo = operand as MethodInfo;

        var paramCount = operandAsMethodInfo?.GetParameters().Length ?? 0;

        var argumentArray = BuildArgumentArray(locals, paramCount, operandAsMethodInfo);

        var returnType = operandAsMethodInfo?.ReturnType ?? typeof(void);
        VReg? returnValue = null;

        MethodResolver.ResolveMethod(operand);
        if (returnType != typeof(void))
        {
            returnValue = locals.NewVirtVar(returnType);

            return isCallVirtual
                ? new VirtualCallReturnOp(returnValue)
                {
                    Resolved = new UnresolvedMethod { Target = operand },
                    Args = argumentArray
                }
                : new CallReturnOp(returnValue)
                {
                    Resolved = new UnresolvedMethod { Target = operand },
                    Args = argumentArray
                };
        }

        return isCallVirtual
            ? new VirtualCallOp
            {
                Resolved = new UnresolvedMethod { Target = operand },
                Args = argumentArray
            }
            : new CallOp
            {
                Resolved = new UnresolvedMethod { Target = operand },
                Args = argumentArray
            };
    }

    private static IValueExpression[] BuildArgumentArray(LocalVariablesStackAndState locals, int paramCount,
        MethodInfo? operandAsMethodInfo)
    {
        var argumentList = new List<IValueExpression>();
        for (var i = 0; i < paramCount; i++) argumentList.Add(locals.Pop());

        if (operandAsMethodInfo != null && !operandAsMethodInfo.IsStatic)
            //makes sure that this pointer is also pushed for non static methods.
            argumentList.Add(locals.Pop());

        argumentList.Reverse();

        var argumentArray = argumentList.ToArray();
        return argumentArray;
    }
}