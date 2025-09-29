using NativeSharp.CodeGen;
using NativeSharp.Operations;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd.Transformers;

static class ConvertOperationTransformer
{
    public static BaseOp TransformConvOperation(string opName, LocalVariablesStackAndState localVariablesStackAndState)
    {
        IValueExpression localVar = localVariablesStackAndState.Pop();
        string mappedSuffix = opName.Split('.')[1];
        Type targetType = mappedSuffix switch
        {
            "i4" => typeof(int),
            "i8" => typeof(long),
            _ => throw new InvalidOperationException($"Cannot cast to: {mappedSuffix}")
        };

        VReg resultVar = localVariablesStackAndState.NewVirtVar(targetType);
        return new UnaryOp(resultVar)
        {
            Operator = opName.CleanupFieldName(),
            ValueExpression = localVar
        };
    }
}