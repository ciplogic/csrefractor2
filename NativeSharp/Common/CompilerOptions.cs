namespace NativeSharp.Common;

public class CompilerOptions
{
    public OptimizationOptions Optimize { get; set; } = new();
    public TimingMainKind TimingMain { get; set; } = TimingMainKind.Millisecond;
}