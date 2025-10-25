using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.BlockOptimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

public class BlockBasedDededuplicatedArrayReads : BlockBasedOptimizationBase
{
    public override bool OptimizeSegment(ArraySegment<BaseOp> segment)
    {

        int containsArrayElementRead = segment.AsSpan().CountOf<LoadElementOp>();
        if (containsArrayElementRead <= 1)
        {
            return false;
        }

        BaseOp[] methodOperations = Method.Operations;
        HashSet<string> changingVariables = CalculateModifiedVariables(segment);

        List<InterestingLoadFieldOp> interestingSetFields = [];
        for (int index = 0; index < segment.Count; index++)
        {
            var op = segment[index];
            if (op is not LoadElementOp loadFieldOp || changingVariables.Contains(loadFieldOp.Array.ToString()!))
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
            LoadElementOp loadElementOp = interestingLoadFieldOp.ArrayRead;
            var rightSideExpression =
                $"{loadElementOp.Array.Code()}.{loadElementOp.Index}";
            if (!variables.TryGetValue(rightSideExpression, out var variable))
            {
                variables[rightSideExpression] = loadElementOp.Left;
                continue;
            }
            changed = true;
            methodOperations[interestingLoadFieldOp.Index] = new AssignOp(loadElementOp.Left, variable);
        }

        return changed;
    }

    record struct InterestingLoadFieldOp(int Index, LoadElementOp ArrayRead);

    private static HashSet<string> CalculateModifiedVariables(Span<BaseOp> methodOperations)
    {
        HashSet<string> result = new HashSet<string>();
        foreach (var op in methodOperations)
        {
            if (op is LoadElementOp { Left: Variable leftVariable })
            {
                result.Add(leftVariable.ToString()!);
            }
        }

        return result;
    }
}