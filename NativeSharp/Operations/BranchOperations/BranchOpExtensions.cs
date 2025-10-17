using NativeSharp.CodeGen;

namespace NativeSharp.Operations.BranchOperations;

internal static class BranchOpExtensions
{
    public static string MapNameToTruthBranch(this string nameOp)
    {
        var cleanedName = nameOp.CleanupFieldName();
        switch (cleanedName)
        {
            case "brfalse_s":
            case "brfalse":
                return "brfalse";
            case "brtrue_s":
            case "brtrue":
                return "brtrue";
            default:
                throw new ArgumentException($"{cleanedName} is not a valid branch name");
        }
    }
}