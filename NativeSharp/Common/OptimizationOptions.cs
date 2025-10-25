using NativeSharp.EscapeAnalysis;

namespace NativeSharp.Common;

public class OptimizationOptions
{
    public bool UseInlining { get; set; } = false;
    public bool UseFieldDeduplication { get; set; } = true;
    public bool FastRefCountMode { get; set; } = true;
    public EscapeAnalysisMode EscapeAnalysisMode { get; set; } = EscapeAnalysisMode.None;
}