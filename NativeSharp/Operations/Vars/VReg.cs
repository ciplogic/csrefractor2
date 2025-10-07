using NativeSharp.Operations.BranchOperations;

namespace NativeSharp.Operations.Vars;

public class VReg : IndexedVariable
{
    public override string ToString() => $"vreg_{CompactNumberWriter.Str(Index)}";

    public override string GenCodeImpl() => $"vreg_{CompactNumberWriter.Str(Index)}";
}