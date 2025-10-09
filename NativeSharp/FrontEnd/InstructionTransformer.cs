using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.FrontEnd.Transformers;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd;

internal class InstructionTransformer
{
    public List<ArgumentVariable> Params { get; } = [];

    public readonly LocalVariablesStackAndState LocalVariablesStackAndState = new();

    private Type ReturnType { get; set; } = typeof(void);

    public BaseOp[] Transform(MethodBase parentMethod)
    {
        BuildLocalVariables(parentMethod);

        Instruction[] instructions2 = MethodBodyReader.GetInstructions(parentMethod);

        List<BaseOp> resultList = new (instructions2.Length);
        for (int index = 0; index < instructions2.Length; index++)
        {
            Instruction instruction = instructions2[index];
            if (LocalVariablesStackAndState.TargetBranches.Contains(instruction.Offset))
            {
                resultList.Add(new LabelOp(instruction.Offset));
            }

            BaseOp transformedOp = TransformOp(instruction);
            if (transformedOp is CompositeOp compositeOp)
            {
                foreach (BaseOp baseOp in compositeOp.Ops)
                {
                    resultList.Add(baseOp);
                }
            }
            else
            {
                resultList.Add(transformedOp);
            }
        }

        BaseOp[] ops = resultList.ToArray();
        ops = PhiFixup.FixupMerges(ops);

        return ops;
    }

    private void BuildLocalVariables(MethodBase parentMethod)
    {
        LocalVariablesStackAndState.BuildLocalVariables(parentMethod);
        Params.Clear();
        Params.AddRange(parentMethod.GetMethodArguments());

        ReturnType = (parentMethod as MethodInfo)?.ReturnType ?? typeof(void);
    }

    private BaseOp TransformOp(Instruction instruction)
    {
        string? opName = instruction.OpCode.Name;
        if (opName == "nop")
        {
            return new CompositeOp([]);
        }

        if (opName == "ret")
        {
            if (ReturnType == typeof(void))
                return new RetOp(null);
            return new RetOp(LocalVariablesStackAndState.Pop());
        }

        if (opName.StartsWith("ld"))
        {
            return LoadOperationsTransformer.TransformLoadOp(instruction, Params, LocalVariablesStackAndState);
        }

        if (opName.StartsWith("b"))
        {
            return TransformBranchOperation(instruction, opName);
        }

        if (opName.StartsWith("conv"))
        {
            return ConvertOperationTransformer.TransformConvOperation(opName, LocalVariablesStackAndState);
        }

        if (opName.StartsWith("call"))
        {
            return CallOperationsTransformer.TransformCallOp(LocalVariablesStackAndState, instruction);
        }

        if (opName.StartsWith("stloc"))
        {
            return TransformStoreOp(instruction);
        }

        if (opName == "stfld")
        {
            return TransformStoreField(instruction, LocalVariablesStackAndState);
        }

        if (opName.StartsWith("stelem"))
        {
            return TransformStoreElement(instruction, LocalVariablesStackAndState);
        }

        if (LogicalBinaryOp.Contains(opName))
        {
            return TransformLogicalBinaryOp(instruction);
        }

        if (BinaryOps.Contains(opName))
        {
            return TransformBinaryOp(instruction);
        }

        if (UnaryOps.Contains(opName))
        {
            return TransformUnaryOp(instruction);
        }

        if (opName.StartsWith("new"))
        {
            return TransformNewOps.TransformNewDeclarations(instruction, LocalVariablesStackAndState);
        }

        if (opName == "pop")
        {
            LocalVariablesStackAndState.Pop();
            return new CompositeOp([]);
        }

        if (opName == "dup")
        {
            return TransformDup();
        }


        throw new InvalidOperationException(opName);
    }

    private BaseOp TransformStoreElement(Instruction instruction,
        LocalVariablesStackAndState localVariablesStackAndState)
    {
        IValueExpression valueToSet = localVariablesStackAndState.Pop();
        IValueExpression index = localVariablesStackAndState.Pop();
        IndexedVariable arrPtr = (IndexedVariable)localVariablesStackAndState.Pop();

        return new StoreElementOp(arrPtr, index, valueToSet);
    }

    private BaseOp TransformStoreField(Instruction instruction, LocalVariablesStackAndState localVariablesStackAndState)
    {
        FieldInfo fieldInfo = (FieldInfo)instruction.Operand;
        IValueExpression valueToSet = localVariablesStackAndState.Pop();
        IndexedVariable thisPtr = (IndexedVariable)localVariablesStackAndState.Pop();

        return new StoreFieldOp(thisPtr, valueToSet, fieldInfo.Name.CleanupFieldName());
    }

    private BaseOp TransformDup()
    {
        IValueExpression original = LocalVariablesStackAndState.Pop();
        VReg vreg1 = LocalVariablesStackAndState.NewVirtVar(original.ExpressionType);
        VReg vreg2 = LocalVariablesStackAndState.NewVirtVar(original.ExpressionType);
        AssignOp assignOp1 = new(vreg1, original);
        AssignOp assignOp2 = new(vreg2, original);

        return new CompositeOp([assignOp1, assignOp2]);
    }

    static string[] BranchOps = ["brfalse", "brtrue"];

    private static string[] BoolBinaryOperations = ["blt", "bgt", "blt.s", "bgt.s"];

    private BaseOp TransformBranchOperation(Instruction instruction, string opName)
    {
        if (BoolBinaryOperations.Contains(opName))
        {
            return TransformBoolBinaryOp(instruction, opName, LocalVariablesStackAndState);
        }

        bool isConditional = BranchOps.Any(opName.StartsWith);

        Instruction targetInstruction = (Instruction)instruction.Operand;
        int targetInstructionOffset = targetInstruction.Offset;
        if (!isConditional)
        {
            return new GotoOp(targetInstructionOffset);
        }

        return new BranchOp(targetInstructionOffset, opName,
            isConditional ? LocalVariablesStackAndState.Pop() : null);
    }

    private static BaseOp TransformBoolBinaryOp(Instruction instruction, string opName, LocalVariablesStackAndState localVariablesStackAndState)
    {
        IValueExpression right = localVariablesStackAndState.Pop();
        IValueExpression leftOp = localVariablesStackAndState.Pop();
        VReg left = localVariablesStackAndState.NewVirtVar<bool>();
        BinaryOp binaryOp = new BinaryOp(left, opName)
        {
            LeftExpression = leftOp,
            RightExpression = right
        };
        Instruction operand = (Instruction)instruction.Operand;
        BranchOp branchOp = new(operand.Offset, "brtrue", left);
        CompositeOp combinedOp = new([binaryOp, branchOp]);

        return combinedOp;
    }

    private BaseOp TransformBinaryOp(Instruction instruction)
    {
        IValueExpression rightOp = LocalVariablesStackAndState.Pop();
        IValueExpression leftOp = LocalVariablesStackAndState.Pop();
        VReg left = LocalVariablesStackAndState.NewVirtVar(leftOp.ExpressionType);
        return new BinaryOp(left, instruction.OpCode.Name!)
        {
            LeftExpression = leftOp,
            RightExpression = rightOp,
        };
    }

    private BaseOp TransformUnaryOp(Instruction instruction)
    {
        IValueExpression leftOp = LocalVariablesStackAndState.Pop();
        VReg left = LocalVariablesStackAndState.NewVirtVar(leftOp.ExpressionType);
        return new UnaryOp(left)
        {
            ValueExpression = leftOp,
            Operator = $"{instruction.OpCode.Name!}"
        };
    }


    private BaseOp TransformLogicalBinaryOp(Instruction instruction)
    {
        IValueExpression rightOp = LocalVariablesStackAndState.Pop();
        IValueExpression leftOp = LocalVariablesStackAndState.Pop();
        VReg left = LocalVariablesStackAndState.NewVirtVar(leftOp.ExpressionType);
        return new BinaryOp(left, instruction.OpCode.Name!)
        {
            LeftExpression = leftOp,
            RightExpression = rightOp
        };
    }

    string[] LogicalBinaryOp = ["cgt", "ceq", "clt", "cle", "cgt.un", "clt.un", "ceq.un", "cne.un"];

    string[] BinaryOps = ["rem", "add", "sub", "mul", "div"];
    string[] UnaryOps = ["neg"];

    private BaseOp TransformStoreOp(Instruction instruction)
    {
        string? opName = instruction.OpCode.Name;
        string[] components = opName.Split('.');
        int index = 0;
        if (components[0] == ("stloc"))
        {
            if (!int.TryParse(components[1], out index))
            {
                if (components[1] == "s")
                {
                    LocalVariableInfo localVar = (LocalVariableInfo)instruction.Operand;
                    index = localVar.LocalIndex;
                }
                else
                {
                    index = (int)instruction.Operand;
                }
            }

            IndexedVariable leftVar = LocalVariablesStackAndState.LocalVariables[index];

            IValueExpression expression = LocalVariablesStackAndState.Pop();
            AssignOp assignOp = new(leftVar, expression);
            return assignOp;
        }

        throw new InvalidOperationException(opName);
    }
}