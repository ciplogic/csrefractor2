using System.Reflection;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd.Transformers;

class LocalVariablesStackAndState(int vregIndex = 0)
{
    public List<IndexedVariable> LocalVariables { get; } = [];
    private readonly Stack<IValueExpression> variableStack = new();
    public int[] TargetBranches = [];
    private int vregIndex = vregIndex;


    public VReg NewVirtVar<T>() => NewVirtVar(typeof(T));

    public VReg NewVirtVar(Type varType)
    {
        VReg virtVar = new VReg()
        {
            Index = vregIndex++,
            ExpressionType = varType
        };
        LocalVariables.Add(virtVar);
        variableStack.Push(virtVar);
        return virtVar;
    }

    public void BuildLocalVariables(MethodBase parentMethod)
    {
        vregIndex = 0;
        variableStack.Clear();
        LocalVariables.Clear();

        IList<LocalVariableInfo> locals = parentMethod.GetMethodBody()!.LocalVariables ?? [];
        foreach (LocalVariableInfo localVariableInfo in locals)
        {
            LocalVariable localVariable = new LocalVariable()
            {
                Index = localVariableInfo.LocalIndex,
                ExpressionType = localVariableInfo.LocalType
            };
            LocalVariables.Add(localVariable);
        }

        Instruction[] instructions2 = MethodBodyReader.GetInstructions(parentMethod);
        TargetBranches = instructions2.BuildTargetBranches();
    }

    public IValueExpression Pop() => variableStack.Pop();
}