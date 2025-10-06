using NativeSharp.Operations.Vars;

namespace NativeSharp.Operations.Common;

public class CilOperationsMethod : NativeMethodBase
{
    public BaseOp[] Operations { get; set; } = [];
    public IndexedVariable[] Locals { get; set; } = [];

    public MetadataAnalysis Analysis = new();
}

public class MetadataAnalysis
{
    public AnalysisProgress EscapeAnalysis;
    public AnalysisProgress CallsResolved;
}

public enum AnalysisProgress
{
    NotDone,
    InProgress,
    Done
}