using System.Reflection;
using NativeSharp.FrontEnd;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;
using NativeSharp.Operations.Vars;
using NativeSharp.Resolving;

namespace NativeSharp.CodeGen;

public class CodeGenerator
{
    private CodeGenToFile Code { get; } = new("output.cpp");

    public void WriteMethodsAndMain(string entryPoint)
    {
        Code.AddLine("#include \"native_sharp.hpp\"");

        AddNativeCppHeaders(MethodResolver.MethodCache);
        WriteReferencedTypes();
        WriteInitialCode();

        foreach (BaseNativeMethod method in MethodResolver.MethodCache.Values)
        {
            WriteCilMethodHeader(method);
        }

        WriteMainBody(entryPoint);

        WriteMarshallCode();

        foreach (BaseNativeMethod method in MethodResolver.MethodCache.Values)
        {
            switch (method)
            {
                case CilNativeMethod cilMethod:
                    WriteCilMethod(cilMethod);
                    break;
                case CppNativeMethod cppMethod:
                    WriteCppMethod(cppMethod);
                    break;
            }

            Code.AddLine();
        }

        WriteStringPool();

        Code.WriteToFile();
    }

    private void WriteMarshallCode()
    {
        Code.AddLine(
            """
            namespace {
              std::vector<uint8_t> marshallStringCharStar(System_String* text) {
                std::vector<uint8_t> result;
                uint8_t* dataPtr = text->Data->data();
                int textLen = text->Data->size();
                for (int i = 0; i < textLen; i++) {
                  result.push_back(dataPtr[i]);
                }
                result.push_back(0);
                if (text->Coder) {
                  result.push_back(0);
                }
            
                return result;
              };
            }
            """
            );
    }

    private void AddNativeCppHeaders(Dictionary<MethodBase, BaseNativeMethod> methodCacheValues)
    {
        HashSet<string> headersHash = [];
        Code.AddLine("// headers imported by native methods");
        foreach (BaseNativeMethod method in methodCacheValues.Values)
        {
            if (method is not CppNativeMethod cppMethod)
            {
                continue;
            }

            var headers = cppMethod.Content.Headers;
            foreach (var header in headers)
            {
                if (headersHash.Add(header))
                {
                    Code.AddLine($"#include {header}");
                }
            }
        }

        Code.AddLine();
    }

    private void WriteCppMethod(CppNativeMethod cppMethod)
    {
        var code = cppMethod.Content;
        Code.AddLine($"{cppMethod.MangledMethodHeader()} {{");
        Code.AddLine(code.MethodBody);
        Code.AddLine("}");
    }

    private void WriteMainBody(string entryPoint, string args = "") =>
        Code
            .AddLine("int main() {")
            .AddLine(entryPoint + "();")
            .AddLine("return 0;")
            .AddLine("}");

    public void WriteReferencedTypes()
    {
        Dictionary<Type, Type> mappedTypes = MethodResolver.MappedType.Straight;
        foreach (KeyValuePair<Type, Type> kv in mappedTypes)
        {
            Code.AddLine($"struct {kv.Value.Mangle(RefKind.Value)};");
        }

        foreach (KeyValuePair<Type, Type> kv in mappedTypes)
        {
            WriteFieldsOfType(kv);
        }
    }

    private void WriteFieldsOfType(KeyValuePair<Type, Type> kv)
    {
        Code.AddLine($"struct {kv.Value.Mangle(RefKind.Value)} {{");
        Type mappedType = kv.Key;
        var regularFields = mappedType.GetFields();
        FieldInfo[] fieldsOfType = mappedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo variable in fieldsOfType)
        {
            if (variable.IsStatic)
            {
                continue;
            }

            Code.AddLine($"{variable.FieldType.Mangle()} {variable.Name};");
        }

        Code.AddLine("};");
    }

    private void WriteInitialCode()
    {
        Code.AddLine(
            """
            namespace {
                Ref<System_String> _clr_str(int index);
            }
            """);
    }

    private void WriteInstructions(BaseOp[] instructions)
    {
        foreach (BaseOp instruction in instructions)
        {
            Code.AddLine(instruction.GenCode());
        }
    }

    private void WriteCilMethodHeader(BaseNativeMethod cilNativeMethod)
    {
        Code.AddLine($"{cilNativeMethod.MangledMethodHeader()};");
    }

    private void WriteCilMethod(CilNativeMethod cilNativeMethod)
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
        foreach (IndexedVariable localVariable in cilMethodLocals)
        {
            Code.AddLine($"{localVariable.ExpressionType.Mangle()} {localVariable.GenCodeImpl()};");
        }
    }

    private void WriteStringPool()
    {
        StringPool stringPool = StringPool.Instance;
        List<int> startPositions = [];
        List<int> lenPos = [];
        List<byte> joinedTexts = [];
        int startPos = 0;
        foreach (byte[] utf8Text in stringPool.Values)
        {
            startPositions.Add(startPos);
            lenPos.Add(utf8Text.Length);
            startPos += utf8Text.Length;
            joinedTexts.AddRange(utf8Text);
        }

        Code
            .AddLine("namespace {")
            .AddLine($"    RefArr<int> _coders = makeArr<int> ({{{string.Join(',', stringPool.Coders)}}});")
            .AddLine($"    RefArr<int> _startPos = makeArr<int> ({{{string.Join(',', startPositions)}}});")
            .AddLine($"    RefArr<int> _lengths = makeArr<int> ({{{string.Join(',', lenPos)}}});")
            .AddLine(
                $"    RefArr<uint8_t> _joinedTexts = makeArr<uint8_t> ({{{string.Join(',', joinedTexts)}}});")
            .AddLine("""
                         Ref<System_String> _clr_str(int index) {
                            return Texts_FromIndex(index, _coders, _startPos, _lengths, _joinedTexts);
                         }
                     }
                     """);
    }
}