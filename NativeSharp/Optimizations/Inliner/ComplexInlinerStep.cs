using NativeSharp.Common;
using NativeSharp.FrontEnd.Transformers;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.Common;
using NativeSharp.Optimizations.DeadCodeElimination;

namespace NativeSharp.Optimizations.Inliner;

public class ComplexInlinerStep
{
    public bool InlineComplex(CilNativeMethod cilNativeMethod, int row)
    {
        var target = InlinerExtensions.GetTargetCall(cilNativeMethod.Instructions[row]);
        if (target is null)
        {
            return false;
        }

        var startVregIndex = MaxIndexOfVariable(cilNativeMethod) + 1;

        Dictionary<IValueExpression, IValueExpression> fromToVariables = CreateVariablesTable(target, startVregIndex);
        (BaseOp[] OpsCloned, IndexedVariable[] newVars, IValueExpression? returnValueExpression) opsToInline =
            GetOpsToInline(target, fromToVariables);
        if (opsToInline.OpsCloned.Length == 0)
        {
            CilMethodExtensions.RemoveIndices(cilNativeMethod, [row]);
            return true;
        }

        if (opsToInline.OpsCloned.Length > 5)
        {
            return false;
        }

        ApplyInline(cilNativeMethod, row, opsToInline.OpsCloned, opsToInline.newVars,
            opsToInline.returnValueExpression);

        return true;
    }

    private void ApplyInline(CilNativeMethod cilNativeMethod, int row, BaseOp[] OpsCloned, IndexedVariable[] newVars,
        IValueExpression? returnValueExpression)
    {
        var newOps = new List<BaseOp>();

        newOps.AddRange(cilNativeMethod.Instructions.Take(row));
        var argumentsOps = GetCallArguments(cilNativeMethod.Instructions[row]);
        for (var index = 0; index < argumentsOps.Length; index++)
        {
            var argument = argumentsOps[index];
            var leftSide = newVars[index];
            newOps.Add(new AssignOp(leftSide, argument));
        }
        newOps.AddRange(OpsCloned);
        if (returnValueExpression is not null)
        {
            var callRetOp = (CallReturnOp)cilNativeMethod.Instructions[row];
            newOps.Add(new AssignOp(callRetOp.Left, returnValueExpression));
        }

        newOps.AddRange(cilNativeMethod.Instructions.Skip(row + 1));

        cilNativeMethod.Locals = cilNativeMethod.Locals.Concat(newVars).ToArray();
        cilNativeMethod.Instructions = newOps.ToArray();
    }

    private static (BaseOp[] OpsCloned, IndexedVariable[], IValueExpression? returnValueExpression) GetOpsToInline(
        CilNativeMethod target, Dictionary<IValueExpression, IValueExpression> fromToVariables)
    {
        var labelsTable = CreateLabelsTable(target);
        var opsToInline = BuildOpsToInline(target, fromToVariables, labelsTable);

        var lastOp = opsToInline.Last();
        if (lastOp is not RetOp retOp)
        {
            throw new InvalidDataException("No final return");
        }

        opsToInline = opsToInline.Take(opsToInline.Length - 1).ToArray();

        return (opsToInline, fromToVariables.Values.Select(it => (IndexedVariable)it).ToArray(), retOp.ValueExpression);
    }

    private static BaseOp[] BuildOpsToInline(CilNativeMethod target,
        Dictionary<IValueExpression, IValueExpression> fromToVariables, Dictionary<int, int> labelsTable)
    {
        BaseOp[] opsToInline = target.Instructions.SelectToArray(op => op.Clone());
        foreach (var op in opsToInline)
        {
            InstructionUsages.UpdateUsagesAndDefinitions(op, fromToVariables);
        }

        foreach (var op in opsToInline)
        {
            if (op is OffsetOp offsetOp)
            {
                offsetOp.Offset = labelsTable[offsetOp.Offset];
            }
        }

        return opsToInline;
    }

    private static Dictionary<IValueExpression, IValueExpression> CreateVariablesTable(CilNativeMethod targets,
        int startIndex)
    {
        Dictionary<IValueExpression, IValueExpression> variablesTable =
            new Dictionary<IValueExpression, IValueExpression>();
        var localVariablesStack = new LocalVariablesStackAndState(startIndex);

        foreach (var argumentVariable in targets.Args)
        {
            VReg variableToMapTo = localVariablesStack.NewVirtVar(argumentVariable.ExpressionType);
            variablesTable[argumentVariable] = variableToMapTo;
        }

        foreach (IndexedVariable variable in targets.Locals)
        {
            VReg variableToMapTo = localVariablesStack.NewVirtVar(variable.ExpressionType);
            variablesTable[variable] = variableToMapTo;
        }
        return variablesTable;
    }


    static int MaxIndexOfVariable(CilNativeMethod cilNativeMethod)
    {
        int result = 0;
        foreach (var indexVariable in cilNativeMethod.Locals)
        {
            result = Math.Max(result, indexVariable.Index);
        }

        return result;
    }

    private static int MaxLabelIndex(CilNativeMethod cilNativeMethod)
    {
        int result = 0;
        foreach (var op in cilNativeMethod.Instructions)
        {
            if (op is LabelOp labelOp)
            {
                result = Math.Max(result, labelOp.Offset);
            }
        }

        return result;
    }

    private static IValueExpression[] GetCallArguments(BaseOp op)
    {
        if (op is CallOp callOp)
        {
            return callOp.Args;
        }
        var callRetOp = (CallReturnOp)op;
        return callRetOp.Args;
    }

    private static Dictionary<int, int> CreateLabelsTable(CilNativeMethod cilNativeMethod)
    {
        Dictionary<int, int> result = [];
        int startIndex = MaxLabelIndex(cilNativeMethod) + 1;

        foreach (var op in cilNativeMethod.Instructions)
        {
            if (op is LabelOp labelOp)
            {
                result[labelOp.Offset] = startIndex++;
            }
        }

        return result;
    }
}