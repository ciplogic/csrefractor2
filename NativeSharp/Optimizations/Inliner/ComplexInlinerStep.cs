using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.FrontEnd.Transformers;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.Common;

namespace NativeSharp.Optimizations.Inliner;

public class ComplexInlinerStep
{
    public static bool InlineComplex(CilOperationsMethod cilOperationsMethod, int row)
    {
        var target = InlinerExtensions.GetTargetCall(cilOperationsMethod.Operations[row]);
        if (target is null)
        {
            return false;
        }

        var startVregIndex = Math.Max(MaxIndexOfVariable(cilOperationsMethod), MaxIndexOfVariable(target)) + 1;

        Dictionary<IValueExpression, IValueExpression> fromToVariables = CreateVariablesTable(target, startVregIndex);
        (BaseOp[] OpsCloned, IndexedVariable[] newVars, IValueExpression? returnValueExpression) opsToInline =
            GetOpsToInline(target, fromToVariables);
        if (opsToInline.OpsCloned.Length == 0)
        {
            CilNativeMethodExtensions.RemoveIndices(cilOperationsMethod, [row]);
            return true;
        }

        if (opsToInline.OpsCloned.Length > 25)
        {
            return false;
        }

        ApplyInline(cilOperationsMethod, row, opsToInline.OpsCloned, opsToInline.newVars,
            opsToInline.returnValueExpression);

        return true;
    }

    private static void ApplyInline(
        CilOperationsMethod cilOperationsMethod, 
        int row, 
        BaseOp[] opsCloned,
        IndexedVariable[] newVars,
        IValueExpression? returnValueExpression)
    {
        var newOps = new List<BaseOp>();

        newOps.AddRange(cilOperationsMethod.Operations.Take(row));
        var argumentsOps = GetCallArguments(cilOperationsMethod.Operations[row]);
        for (var index = 0; index < argumentsOps.Length; index++)
        {
            var argument = argumentsOps[index];
            var leftSide = newVars[index];
            newOps.Add(new AssignOp(leftSide, argument));
        }

        newOps.AddRange(opsCloned);
        if (returnValueExpression is not null)
        {
            var callRetOp = (CallReturnOp)cilOperationsMethod.Operations[row];
            newOps.Add(new AssignOp(callRetOp.Left, returnValueExpression));
        }

        newOps.AddRange(cilOperationsMethod.Operations.Skip(row + 1));

        cilOperationsMethod.Locals = cilOperationsMethod.Locals.Concat(newVars).ToArray();
        cilOperationsMethod.Operations = newOps.ToArray();
    }

    private static (BaseOp[] OpsCloned, IndexedVariable[], IValueExpression? returnValueExpression) GetOpsToInline(
        CilOperationsMethod target, Dictionary<IValueExpression, IValueExpression> fromToVariables)
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

    private static BaseOp[] BuildOpsToInline(CilOperationsMethod target,
        Dictionary<IValueExpression, IValueExpression> fromToVariables, Dictionary<int, int> labelsTable)
    {
        BaseOp[] opsToInline = target.Operations.SelectToArray(op => op.Clone());
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

    private static Dictionary<IValueExpression, IValueExpression> CreateVariablesTable(CilOperationsMethod targets,
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


    static int MaxIndexOfVariable(CilOperationsMethod cilOperationsMethod)
    {
        int result = 0;
        foreach (var indexVariable in cilOperationsMethod.Locals)
        {
            result = Math.Max(result, indexVariable.Index);
        }

        return result;
    }

    private static int MaxLabelIndex(CilOperationsMethod cilOperationsMethod)
    {
        int result = 0;
        foreach (var op in cilOperationsMethod.Operations)
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

    private static Dictionary<int, int> CreateLabelsTable(CilOperationsMethod cilOperationsMethod)
    {
        Dictionary<int, int> result = [];
        int startIndex = MaxLabelIndex(cilOperationsMethod) + 1;

        foreach (var op in cilOperationsMethod.Operations)
        {
            if (op is LabelOp labelOp)
            {
                result[labelOp.Offset] = startIndex++;
            }
        }

        return result;
    }
}