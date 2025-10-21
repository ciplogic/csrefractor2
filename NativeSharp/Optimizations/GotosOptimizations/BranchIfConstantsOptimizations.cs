using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.Optimizations.GotosOptimizations;

public class BranchIfConstantsOptimizations : OptimizationBase
{
    public override bool Optimize(CilOperationsMethod cilOperationsMethod)
    {
        BaseOp[] ops = cilOperationsMethod.Operations;
        bool result = ReplaceWithGotos(ops);

        int[] removedFalseBranches = RemovedFalseBranchIfs(ops);
        result = result || removedFalseBranches.Length != 0;
        cilOperationsMethod.RemoveIndices(removedFalseBranches);
        return result;
    }

    private static bool ReplaceWithGotos(BaseOp[] ops)
    {
        bool result = false;
        for (int index = 0; index < ops.Length; index++)
        {
            BaseOp baseOp = ops[index];
            if (!IsBranchOpEquivalentWithGoto(baseOp))
            {
                continue;
            }
            BranchOp branchOp = (BranchOp)ops[index];
            ops[index] = new GotoOp(branchOp.Offset);
            result = true;
        }

        
        return result;
    }

    private static int[] RemovedFalseBranchIfs(BaseOp[] ops)
    {
        List<int> result = [];
        for (int index = 0; index < ops.Length; index++)
        {
            BaseOp baseOp = ops[index];
            if (IsBranchOpIsFalse(baseOp))
            {
                result.Add(index);
            }
        }
        return result.ToArray();
    }

    private static bool IsBranchOpIsFalse(BaseOp baseOp)
    {
        if (baseOp is not BranchOp { Condition: ConstantValueExpression constantValueExpression } branchOp)
        {
            return false;
        }

        return branchOp.Name switch
        {
            "brfalse" => IsTruthy(constantValueExpression),
            "brtrue" => IsFalsy(constantValueExpression),
            _ => false
        };
    }

    private static bool IsBranchOpEquivalentWithGoto(BaseOp baseOp)
    {
        if (baseOp is not BranchOp { Condition: ConstantValueExpression constantValueExpression } branchOp)
        {
            return false;
        }

        return branchOp.Name switch
        {
            "brfalse" => IsFalsy(constantValueExpression),
            "brtrue" => IsTruthy(constantValueExpression),
            _ => false
        };
    }

    private static bool IsFalsy(ConstantValueExpression constantValueExpression)
    {
        switch (constantValueExpression.Value)
        {
            case int i:
                return i == 0;
            default:throw new ArgumentException($"{constantValueExpression.Value} is not a valid branch expression");
        }
        return false;
    }
    private static bool IsTruthy(ConstantValueExpression constantValueExpression)
    {
        switch (constantValueExpression.Value)
        {
            case int i:
                return i != 0;
            default: throw new ArgumentException($"{constantValueExpression.Value} is not a valid branch expression");
        }
        return false;
    }
}