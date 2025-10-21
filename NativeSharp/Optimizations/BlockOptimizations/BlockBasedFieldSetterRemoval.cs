using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;
using NativeSharp.Optimizations.BlockOptimizations.Common;

namespace NativeSharp.Optimizations.BlockOptimizations;

internal class BlockBasedFieldSetterRemoval : BlockBasedOptimizationBase
{
    public override bool OptimizeSegment(ArraySegment<BaseOp> segment)
    {
        bool containsFieldSetter = segment.AsSpan().Contains<StoreFieldOp>();
        bool containsFieldGetter = segment.AsSpan().Contains<LoadFieldOp>();

        if (!containsFieldSetter || containsFieldGetter)
        {
            return false;
        }

        HashSet<IndexedVariable> variables = [];

        Dictionary<IndexedVariable, List<(string FieldName, int Index)>> previousValueFieldSet = [];

        for (int index = 0; index < segment.Count; index++)
        {
            BaseOp op = segment[index];
            if (op is LeftOp leftOp)
            {
                if (!leftOp.Left.ExpressionType.IsValueType)
                {
                    variables.Add(leftOp.Left);
                    previousValueFieldSet.Remove(leftOp.Left);
                }
                continue;
            }

            if (op is StoreFieldOp storeFieldOp)
            {
                if (!previousValueFieldSet.TryGetValue(storeFieldOp.ThisPtr, out List<(string FieldName, int Index)>? previousValues))
                {
                    List<(string, int)> items = [(storeFieldOp.FieldName, index)];
                    previousValueFieldSet[storeFieldOp.ThisPtr] = items;
                    continue;
                }

                (string FieldName, int Index) previousFoundValue =
                    previousValues.FirstOrDefault(x => x.FieldName == storeFieldOp.FieldName);
                if (string.IsNullOrEmpty(previousFoundValue.FieldName))
                {
                    previousValues.Add((storeFieldOp.FieldName, index));
                    continue;
                }
                Method.RemoveIndices([previousFoundValue.Index + segment.Offset]);
                return true;
            }
        }

        return false;
    }
    
    
}