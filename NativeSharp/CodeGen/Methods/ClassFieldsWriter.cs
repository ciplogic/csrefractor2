using System.Reflection;

namespace NativeSharp.CodeGen.Methods;

public record ClassFieldsWriter(CodeGenToFile Code, Type sourceType, Type mappedType)
{
    public void WriteFieldsOfType()
    {
        FieldInfo[] fieldsOfType =
            mappedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        string className = sourceType.Mangle(RefKind.Value);
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
            var fieldCurrentType = variable.FieldType.Mangle();
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

        var variableNames = string.Join(',', fieldNames);
        Code.AddLine($"{fieldType} {variableNames};", 2);
        fieldNames.Clear();
    }
}