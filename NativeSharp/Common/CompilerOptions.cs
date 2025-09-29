namespace NativeSharp;

public class CompilerOptions
{
    public bool Optimize { get; set; } = true;
    public TimingMainKind TimingMain { get; set; } = TimingMainKind.Millisecond;
}

public enum TimingMainKind
{
    None,
    Nanosecond,
    Millisecond,
    Second,
    Microsecond
}