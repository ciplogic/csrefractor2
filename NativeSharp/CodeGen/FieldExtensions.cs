namespace NativeSharp.CodeGen;

internal static class FieldExtensions {
    public static string CleanupFieldName(this string name) =>
        name
            .ReplaceQuick('.', '_')
            .ReplaceQuick('<', '_')
            .ReplaceQuick('>', '_');

    private static string ReplaceQuick(this string text, char from, char to) => !text.Contains(from) ? text : text.Replace(from, to);
}