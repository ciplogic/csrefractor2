using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Common;

public class CilOperationsMethod : BaseNativeMethod
{
    public BaseOp[] Operations { get; set; } = [];
    public IndexedVariable[] Locals { get; set; } = [];

    public MetadataAnalysis Analysis = new();
}

public class MetadataAnalysis
{
    public EaProgress IsEaAnalysisDone;
}

public enum EaProgress
{
    NotDone,
    InProgress,
    Done
}