using System.Reflection;

namespace NativeSharp.CodeGen.Methods;

public record ClassFieldsWriter(CodeGenToFile Code, Type? SourceType, Type? MappedType)
{
    public void WriteFieldsOfType()
    {
        FieldInfo[] fieldsOfType =
            MappedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        string className = SourceType.Mangle(RefKind.Value);
        if (fieldsOfType.Length == 0)
        {
            Code.AddLine($"struct {className} {{}};");
            return;
        }
        Code.AddLine($"struct {className} {{");
        string currentType = String.Empty;
        List<string> fieldNames = [];
        foreach (FieldInfo variable in fieldsOfType)
        {
            string fieldCurrentType = variable.FieldType.Mangle();
            if (fieldCurrentType != currentType)
            {
                WriteCurrentFields(currentType, fieldNames);
            }

            fieldNames.Add(variable.Name.CleanupFieldName());
            currentType = fieldCurrentType;
        }

        WriteCurrentFields(currentType, fieldNames);
        Code.AddLine("};");
    }

    private void WriteCurrentFields(string fieldType, List<string> fieldNames)
    {
        if (fieldNames.Count == 0)
        {
            return;
        }

        string variableNames = string.Join(',', fieldNames);
        Code.AddLine($"{fieldType} {variableNames};", 2);
        fieldNames.Clear();
    }
}