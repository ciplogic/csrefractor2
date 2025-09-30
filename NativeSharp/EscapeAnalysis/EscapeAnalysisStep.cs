using System.Diagnostics;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations;
using NativeSharp.Optimizations.Common;

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

    private static void Analyze(CilNativeMethod cilMethod)
    {
        Dictionary<string, Variable> variables = PopulateVariables(cilMethod);
        var instructions = cilMethod.Instructions;
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
        if (instruction is RetOp retOp)
        {
            var valueExpressionText = retOp.ValueExpression?.Code() ?? string.Empty;
            UpdateVarUsage(valueExpressionText, variables, EscapeKind.Escapes);
            return;
        }

        if (instruction is StoreFieldOp storeFieldOp)
        {
            var storeElement = storeFieldOp.ValueToSet.Code();
            UpdateVarUsage(storeElement, variables, EscapeKind.Escapes);
            return;
        }

        if (instruction is StoreElementOp storeElementOp)
        {
            var storeElement = storeElementOp.ValueToSet.Code();
            UpdateVarUsage(storeElement, variables, EscapeKind.Escapes);
            return;
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

    private static Dictionary<string, Variable> PopulateVariables(CilNativeMethod cilMethod)
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