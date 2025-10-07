namespace NativeSharp.CodeGen;

internal static class CodeGeneratorBaseTypes
{
    public static void DefaultTypeMappings()
    {
        Register( typeof(int), "int32_t");
        Register( typeof(uint), "uint32_t");
        Register( typeof(short), "int16_t");
        Register( typeof(ushort), "uint16_t");
        Register( typeof(byte), "uint8_t");
        Register( typeof(bool), "bool");
        Register( typeof(float), "float");
        Register( typeof(double), "double");
        Register( typeof(void), "void");
    }
    
    public static readonly Dictionary<string, string> BaseTypeMappings = new();

    public static string SimpleTypeMap(this string name) 
        => BaseTypeMappings.TryGetValue(name, out string mapped) 
            ? mapped
            : name;

    private static void Register(Type p0, string simpleType)
    {
        BaseTypeMappings[p0.Mangle()] = simpleType;
    }

    public static void GenerateNativeMappings()
    {
        CodeGenToFile nativeSharpPrimitives = new CodeGenToFile("native_sharp_primitives.hpp");
        nativeSharpPrimitives.AddLine("#pragma once");
        nativeSharpPrimitives.AddLine();
        nativeSharpPrimitives.AddLine("#include <cstdint>");
        nativeSharpPrimitives.AddLine();
        foreach (KeyValuePair<string, string> item in BaseTypeMappings)
        {
            FormattedLine(nativeSharpPrimitives, item);
        }

        nativeSharpPrimitives.WriteToFile();
    }

    private static void FormattedLine(CodeGenToFile nativeSharpPrimitives, KeyValuePair<string, string> item)
    {
        nativeSharpPrimitives.AddLine($"using {item.Key} = {item.Value};");
    }
}