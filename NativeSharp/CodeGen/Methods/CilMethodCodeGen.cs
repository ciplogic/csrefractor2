using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;

namespace NativeSharp.CodeGen.Methods;

class CilMethodCodeGen(CodeGenToFile Code)
{
    VariablesBulkWriter _writer = new();

    public void WriteCilMethod(CilNativeMethod cilNativeMethod)
    {
        string methodHeader = cilNativeMethod.MangledMethodHeader();
        Code.AddLine(methodHeader);

        Code.AddLine("{");
        WriteLocals(cilNativeMethod.Locals);
        WriteInstructions(cilNativeMethod.Instructions);
        Code.AddLine("}");
    }

    private void WriteLocals(IndexedVariable[] cilMethodLocals)
    {
        _writer.Clear();
        _writer.Populate(cilMethodLocals);
        Code.AddLine(_writer.Write());
    }

    private void WriteInstructions(BaseOp[] instructions)
    {
        foreach (BaseOp instruction in instructions)
        {
            Code.AddLine(instruction.GenCode(), 2);
        }
    }
}