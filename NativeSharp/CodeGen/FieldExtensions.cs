namespace NativeSharp.CodeGen;

static class FieldExtensions {
    public static string CleanupFieldName(this string name)
    {
        return
            name
                .Replace('.', '_')
                .Replace('<', '_')
                .Replace('>', '_');

    }
}