using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.BlockOptimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

public class BlockBasedDededuplicatedFieldReads : BlockBasedOptimizationBase
{
    public override bool OptimizeSegment(ArraySegment<BaseOp> segment)
    {
        int containsArrayElementRead = segment.AsSpan().CountOf<LoadFieldOp>();
        if (containsArrayElementRead <= 1)
        {
            return false;
        }

        BaseOp[] methodOperations = Method.Operations;
        HashSet<string> changingVariables = CalculateModifiedVariables(methodOperations);

        List<InterestingLoadFieldOp> interestingSetFields = [];
        for (int index = 0; index < segment.Count; index++)
        {
            var op = segment[index];
            if (op is not LoadFieldOp loadFieldOp || changingVariables.Contains(loadFieldOp.ThisPtr.ToString()!))
            {
                continue;
            }

            interestingSetFields.Add(new InterestingLoadFieldOp(index + segment.Offset, loadFieldOp));
        }

        if (interestingSetFields.Count <= 1)
        {
            return false;
        }

        bool changed = false;
        Dictionary<string, IndexedVariable> variables = [];
        foreach (InterestingLoadFieldOp interestingLoadFieldOp in interestingSetFields)
        {
            var rightSideExpression =
                $"{interestingLoadFieldOp.LoadField.ThisPtr.Code()}.{interestingLoadFieldOp.LoadField.FieldName}";
            if (!variables.TryGetValue(rightSideExpression, out var variable))
            {
                variables[rightSideExpression] = interestingLoadFieldOp.LoadField.Left;
                continue;
            }
            changed = true;
            methodOperations[interestingLoadFieldOp.Index] = new AssignOp(interestingLoadFieldOp.LoadField.Left, variable);
        }

        return changed;
    }

    record struct InterestingLoadFieldOp(int Index, LoadFieldOp LoadField);

    private static HashSet<string> CalculateModifiedVariables(Span<BaseOp> methodOperations)
    {
        HashSet<string> result = new HashSet<string>();
        foreach (var op in methodOperations)
        {
            if (op is LeftOp { Left: Variable leftVariable })
            {
                result.Add(leftVariable.ToString()!);
            }
        }

        return result;
    }
}