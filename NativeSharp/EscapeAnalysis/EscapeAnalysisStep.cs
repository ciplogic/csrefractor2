using System.Reflection;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.Common;
using NativeSharp.Optimizations.Inliner;

namespace NativeSharp.EscapeAnalysis;

public class EscapeAnalysisStep
{
    public static void ApplyStaticAnalysis()
    {
        CilOperationsMethod[] cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        foreach (CilOperationsMethod cilMethod in cilMethods)
        {
            Analyze(cilMethod);
        }
    }

    private static void Analyze(CilOperationsMethod cilMethod)
    {
        Dictionary<string, Variable> variables = PopulateVariables(cilMethod);
        BaseOp[] instructions = cilMethod.Operations;
        foreach (BaseOp instruction in instructions)
        {
            IEnumerable<string> usages = InstructionUsages.GetUsagesOf(instruction);
            foreach (string usage in usages)
            {
                UpdateVarUsage(usage, variables, EscapeKind.Local);
            }

            AddEscapingInstruction(instruction, variables);
        }

        cilMethod.Analysis.IsEaAnalysisDone = true;
    }

    private static void AddEscapingInstruction(BaseOp instruction, Dictionary<string, Variable> variables)
    {
        switch (instruction)
        {
            case RetOp retOp:
            {
                string valueExpressionText = retOp.ValueExpression?.Code() ?? string.Empty;
                UpdateVarUsage(valueExpressionText, variables, EscapeKind.Escapes);
                return;
            }
            case StoreFieldOp storeFieldOp:
            {
                string storeElement = storeFieldOp.ValueToSet.Code();
                UpdateVarUsage(storeElement, variables, EscapeKind.Escapes);
                return;
            }
            case StoreElementOp storeElementOp:
            {
                string storeElement = storeElementOp.ValueToSet.Code();
                UpdateVarUsage(storeElement, variables, EscapeKind.Escapes);
                return;
            }
            case CallOp callOp:
                UpdateArgsVariableUsage(callOp.Args, callOp.TargetMethod, variables);
                return;
            case CallReturnOp callReturnOp:
                UpdateArgsVariableUsage(callReturnOp.Args, callReturnOp.TargetMethod, variables);
                UpdateVarUsage(callReturnOp.Left.Code(), variables, EscapeKind.Escapes);
                return;
        }
    }

    private static void UpdateArgsVariableUsage(
        IValueExpression[] callOpArgs,
        MethodBase targetMethod,
        Dictionary<string, Variable> variables)
    {
        CilOperationsMethod? cilMethod = InlinerExtensions.ResolvedMethod(targetMethod);
        if (cilMethod is not null)
        {
            if (!cilMethod.Analysis.IsEaAnalysisDone)
            {
                if (InlinerExtensions.IsSimpleMethod(cilMethod))
                {
                    Analyze(cilMethod);
                }
            }

            for (int index = 0; index < callOpArgs.Length; index++)
            {
                IValueExpression arg = callOpArgs[index];
                ArgumentVariable methodArg = cilMethod.Args[index];
                UpdateVarUsage(arg.Code(), variables, methodArg.EscapeResult);
            }
        }
        else
        {
            string[] args = callOpArgs.SelectToArray(x => x.Code());
            foreach (string arg in args)
            {
                UpdateVarUsage(arg, variables, EscapeKind.Escapes);
            }
        }
    }


    private static void UpdateVarUsage(string usage, Dictionary<string, Variable> variables, EscapeKind valueToSet)
    {
        if (!variables.TryGetValue(usage, out Variable? variable))
        {
            return;
        }

        if (valueToSet > variable.EscapeResult)
        {
            variable.EscapeResult = valueToSet;
        }
    }

    private static Dictionary<string, Variable> PopulateVariables(CilOperationsMethod cilMethod)
    {
        Dictionary<string, Variable> result = new Dictionary<string, Variable>();
        foreach (ArgumentVariable argumentVariable in cilMethod.Args)
        {
            result.Add(argumentVariable.Code(), argumentVariable);
        }

        foreach (IndexedVariable localVariable in cilMethod.Locals)
        {
            result.Add(localVariable.Code(), localVariable);
        }

        return result;
    }
}