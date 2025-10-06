using System.Reflection;
using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.BranchOperations;
using NativeSharp.Operations.FieldsAndIndexing;
using NativeSharp.Operations.Vars;

namespace NativeSharp.FrontEnd;

static class PhiFixup
{
    public static BaseOp[] FixupMerges(BaseOp[] ops)
    {
        int[] indicesGotos = VregGotos(ops);
        foreach (int index in indicesGotos)
        {
            AssignOp gotoAssign = (AssignOp)ops[index - 1];
            GotoOp targetGoto = (GotoOp)ops[index];
            int indexLabel = ops.IndexOfOp<LabelOp>(label => label.Offset == targetGoto.Offset);
            if (indexLabel == -1)
            {
                throw new Exception("Expected label not found");
            }

            if (ops[indexLabel - 1] is not AssignOp labelAssign)
            {
                continue;
            }

            AssignOp afterLabelAssign = (AssignOp)ops[indexLabel + 1];
            VReg sourceVreg = (VReg)gotoAssign.Left;
            labelAssign.Left = sourceVreg;
            afterLabelAssign.Expression = sourceVreg;
        }

        for (int index = 0; index < ops.Length; index++)
        {
            BaseOp op = ops[index];
            if (op is LoadNullOp loadNullOp)
            {
                BaseOp nextRow = ops[index + 1];
                Type leftExpressionType = nextRow.EvaluateRightSideExpression();
                AssignOp assignOp = new AssignOp(loadNullOp.Left,
                    new ConstantValueExpression(null) { ExpressionType = leftExpressionType });
                ops[index] = assignOp;
            }
        }

        return ops;
    }

    static Type EvaluateRightSideExpression(this BaseOp baseOp)
    {
        switch (baseOp)
        {
            case StoreFieldOp storeElementOp:
                Type thisPtr = storeElementOp.ThisPtr.ExpressionType;
                FieldInfo? field = thisPtr.GetField(storeElementOp.FieldName);
                return field!.FieldType;
            default:
                throw new NotImplementedException();
        }
    }

    private static int[] VregGotos(BaseOp[] ops)
    {
        List<int> indices = new List<int>();
        for (int index = 1; index < ops.Length; index++)
        {
            BaseOp op = ops[index];
            BaseOp prevOp = ops[index - 1];

            if (op is GotoOp gotoOp && prevOp is AssignOp assignOp && assignOp.Left is VReg)
            {
                indices.Add(index);
            }
        }

        return indices.ToArray();
    }
}