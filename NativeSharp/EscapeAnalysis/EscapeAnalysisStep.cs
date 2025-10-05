using System.Reflection;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations;
using NativeSharp.Optimizations.Common;
using NativeSharp.Optimizations.Inliner;

namespace NativeSharp.EscapeAnalysis;

public class EscapeAnalysisStep
{
    public static void ApplyStaticAnalysis()
    {
        var cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        foreach (var cilMethod in cilMethods)
        {
            Analyze(cilMethod);
        }
    }

    private static void Analyze(CilOperationsMethod cilMethod)
    {
        Dictionary<string, Variable> variables = PopulateVariables(cilMethod);
        var instructions = cilMethod.Operations;
        foreach (var instruction in instructions)
        {
            var usages = InstructionUsages.GetUsagesOf(instruction);
            foreach (var usage in usages)
            {
                UpdateVarUsage(usage, variables, EscapeKind.Local);
            }

            AddEscapingInstruction(instruction, variables);
        }
    }

    private static void AddEscapingInstruction(BaseOp instruction, Dictionary<string, Variable> variables)
    {
        switch (instruction)
        {
            case RetOp retOp:
            {
                var valueExpressionText = retOp.ValueExpression?.Code() ?? string.Empty;
                UpdateVarUsage(valueExpressionText, variables, EscapeKind.Escapes);
                return;
            }
            case StoreFieldOp storeFieldOp:
            {
                var storeElement = storeFieldOp.ValueToSet.Code();
                UpdateVarUsage(storeElement, variables, EscapeKind.Escapes);
                return;
            }
            case StoreElementOp storeElementOp:
            {
                var storeElement = storeElementOp.ValueToSet.Code();
                UpdateVarUsage(storeElement, variables, EscapeKind.Escapes);
                return;
            }
            case CallOp callOp:
                UpdateArgsVariableUsage(callOp.Args, callOp.TargetMethod, variables);
                return;
            case CallReturnOp callReturnOp:
                UpdateArgsVariableUsage(callReturnOp.Args, callReturnOp.TargetMethod, variables);
                return;
        }
    }

    private static void UpdateArgsVariableUsage(
        IValueExpression[] callOpArgs,
        MethodBase targetMethod,
        Dictionary<string, Variable> variables)
    {
        var cilMethod = InlinerExtensions.ResolvedMethod(targetMethod);
        if (cilMethod is not null)
        {
            for (var index = 0; index < callOpArgs.Length; index++)
            {
                var arg = callOpArgs[index];
                var methodArg = cilMethod.Args[index];
                UpdateVarUsage(arg.Code(), variables, methodArg.EscapeResult);
            }
        }
        else
        {
            var args = callOpArgs.SelectToArray(x => x.Code());
            foreach (var arg in args)
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
        var result = new Dictionary<string, Variable>();
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