using System.Reflection;
using NativeSharp.Cha;
using NativeSharp.Cha.Resolving;
using NativeSharp.CodeGen.Methods;
using NativeSharp.Common;
using NativeSharp.FrontEnd;
using NativeSharp.Lib;
using NativeSharp.Operations.Common;

namespace NativeSharp.CodeGen;

public class CodeGenerator
{
    private CodeGenToFile Code { get; } = new("output.cpp");
    private readonly CilMethodCodeGen cilMethodCodeGen;

    public CodeGenerator()
    {
        cilMethodCodeGen = new CilMethodCodeGen(Code);
    }

    public void WriteMethodsAndMain(string entryPoint, MethodInfo mainMethod)
    {
        Code.AddLine("#include \"native_sharp.hpp\"");

        AddNativeCppHeaders(MethodResolver.MethodCache);
        WriteReferencedTypes();
        WriteInitialCode();

        foreach (BaseNativeMethod method in MethodResolver.MethodCache.Values)
        {
            WriteCilMethodHeader(method);
        }

        WriteMainBody(entryPoint, mainMethod.GetParameters().Length == 1 ? "args" : "");

        foreach (BaseNativeMethod method in MethodResolver.MethodCache.Values)
        {
            switch (method)
            {
                case CilOperationsMethod cilMethod:
                    cilMethodCodeGen.WriteCilMethod(cilMethod);
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

            string[] headers = cppMethod.Content.Headers;
            foreach (string header in headers)
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
        CppNativeContent code = cppMethod.Content;
        Code.AddLine($"{cppMethod.MangledMethodHeader()} {{");
        Code.AddLine(code.MethodBody);
        Code.AddLine("}");
    }

    private void WriteMainBody(string entryPoint, string args = "")
    {
        MainMethodCodeGenerator mainCodeGen = new MainMethodCodeGenerator();
        mainCodeGen.WriteMainMethodBody(Code, entryPoint, TimingMainKind.Millisecond, args);
    }

    public void WriteReferencedTypes()
    {
        Dictionary<Type?, Type?> mappedTypes = ClassHierarchyAnalysis.MappedType.Straight;
        foreach (KeyValuePair<Type?, Type?> kv in mappedTypes)
        {
            Code.AddLine($"struct {kv.Value.Mangle(RefKind.Value)};");
        }

        foreach (KeyValuePair<Type?, Type?> kv in mappedTypes)
        {
            ClassFieldsWriter writer = new ClassFieldsWriter(Code, kv.Value, kv.Key);
            writer.WriteFieldsOfType();
        }
    }

    private void WriteInitialCode()
    {
        Code.AddLine(
            """
            namespace {
                Ref<System_String> _str(int index);
            }
            """);
    }


    private void WriteCilMethodHeader(BaseNativeMethod cilNativeMethod)
    {
        Code.AddLine($"{cilNativeMethod.MangledMethodHeader()};");
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
                         Ref<System_String> _str(int index) {
                            return Texts_FromIndex(index, _coders.get(), _startPos.get(), _lengths.get(), _joinedTexts.get());
                         }
                     }
                     """);
    }
}