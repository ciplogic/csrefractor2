using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.CodeGen.Methods;

internal class CilMethodCodeGen(CodeGenToFile code)
{
    private readonly VariablesBulkWriter writer = new();

    public void WriteCilMethod(CilOperationsMethod cilOperationsMethod)
    {
        string methodHeader = cilOperationsMethod.MangledMethodHeader();
        code.AddLine(methodHeader);

        code.AddLine("{");
        WriteLocals(cilOperationsMethod.Locals);
        WriteInstructions(cilOperationsMethod.Operations);
        code.AddLine("}");
    }

    private void WriteLocals(IndexedVariable[] cilMethodLocals)
    {
        writer.Clear();
        writer.Populate(cilMethodLocals);
        code.AddLine(writer.Write());
    }

    private void WriteInstructions(BaseOp[] instructions)
    {
        foreach (BaseOp instruction in instructions)
        {
            code.AddLine(instruction.GenCode(), 2);
        }
    }
}