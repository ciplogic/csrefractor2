using NativeSharp.Operations.BranchOperations;

namespace NativeSharp.Operations.Vars;

public class ArgumentVariable : IndexedVariable
{
    public string Name { get; set; } = string.Empty;

    public override string GenCodeImpl()
        => string.IsNullOrEmpty(Name)
            ? $"arg_{CompactNumberWriter.Str(Index)}"
            : Name;

    public override string ToString()
    {
        return Code();
    }
}