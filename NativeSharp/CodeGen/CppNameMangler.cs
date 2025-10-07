using System.Reflection;
using NativeSharp.Cha;
using NativeSharp.Cha.Resolving;
using NativeSharp.EscapeAnalysis;
using NativeSharp.Operations.Common;

namespace NativeSharp.CodeGen;

internal static class CppNameMangler
{
    public static string Mangle(this Type? clrType, RefKind refKind = RefKind.Default)
    {
        if (ClassHierarchyAnalysis.MappedType.TryGetValue(clrType, out Type? mappedClrType))
        {
            clrType = mappedClrType;
        }

        if (clrType.IsArray)
        {
            return $"RefArr<{clrType.GetElementType()!.Mangle()}>";
        }

        string fullName = clrType.FullName!;
        refKind = refKind == RefKind.Default ? clrType.IsValueType ? RefKind.Value : RefKind.Ref : refKind;
        string resultMangle = Mangle(fullName);
        return refKind switch
        {
            RefKind.Ref => $"Ref<{resultMangle}>",
            RefKind.Ptr => $"{resultMangle}*",
            _ => resultMangle
        };
    }
    
    public static string Mangle(this Type? clrType, EscapeKind escapeKind)
    {
        if (ClassHierarchyAnalysis.MappedType.TryGetValue(clrType, out Type? mappedClrType))
        {
            clrType = mappedClrType;
        }

        if (clrType.IsValueType)
        {
            return Mangle(clrType.FullName!);
        }

        if (clrType.IsArray)
        {
            if (escapeKind == EscapeKind.Local)
            {
                return $"Arr<{clrType.GetElementType()!.Mangle()}>*";
            }

            return $"RefArr<{clrType.GetElementType()!.Mangle()}>";
        }

        string fullName = clrType.FullName!;
        string resultMangle = Mangle(fullName);
        return escapeKind switch
        {
            EscapeKind.Escapes => $"Ref<{resultMangle}>",
            EscapeKind.Local => $"{resultMangle}*",
            _ => resultMangle
        };
    }

    public static string Mangle(this string fullName) => fullName.CleanupFieldName() .SimpleTypeMap();

    public static string MangleMethodName(this MethodBase method)
    {
        string declaringType = method.DeclaringType.Mangle(RefKind.Value);
        string methodName = "ctor";
        if (method is MethodInfo methodInfo)
        {
            methodName = methodInfo.Name;
        }

        return $"{declaringType}_{methodName}";
    }

    public static string MangledMethodHeader(this NativeMethodBase cil)
    {
        string args = string.Join(", ",
            cil.Args.Select(x => $"{x.ExpressionType.Mangle(x.EscapeResult)} {x.GenCodeImpl()}"));
        string methodHeader =
            $"{cil.Target.MangleMethodReturnType()} {cil.Target.MangleMethodName()}({args})";
        return methodHeader;
    }

    public static string MangleMethodReturnType(this MethodBase method)
    {
        if (method is MethodInfo methodInfo)
        {
            return methodInfo.ReturnType.Mangle();
        }

        return typeof(void).Mangle();
    }
}