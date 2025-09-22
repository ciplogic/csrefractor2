using System.Reflection;

namespace NativeSharp.CodeGen.Methods;

public record ClassFieldsWriter(CodeGenToFile Code, Type sourceType, Type mappedType)
{
    public void WriteFieldsOfType()
    {
        Code.AddLine($"struct {sourceType.Mangle(RefKind.Value)} {{");
        FieldInfo[] fieldsOfType =
            mappedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        string currentType = String.Empty;
        List<string> fieldNames = [];
        foreach (FieldInfo variable in fieldsOfType)
        {
            if (variable.IsStatic)
            {
                continue;
            }

            var fieldCurrentType = variable.FieldType.Mangle();
            if (fieldCurrentType != currentType)
            {
                WriteCurrentFields(fieldCurrentType, fieldNames);
            }

            fieldNames.Add(variable.Name);
            currentType = fieldCurrentType;
        }

        WriteCurrentFields(currentType, fieldNames);
        Code.AddLine("}");
    }

    private void WriteCurrentFields(string fieldType, List<string> fieldNames)
    {
        if (fieldNames.Count == 0)
        {
            return;
        }

        var variableNames = string.Join(", ", fieldNames);
        Code.AddLine($"{fieldType} {variableNames};", 2);
        fieldNames.Clear();
    }
}