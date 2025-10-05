using System.Reflection;
using NativeSharp.Common;

namespace NativeSharp.Cha;

public class ClassHierarchyAnalysis
{
    public static List<Type> RegisteredTypes { get; } = [];
    public static TwoWayDictionary<Type> MappedType { get; } = new();
    public static Type ResolveType(Type targetType)
    {
        if (MappedType.TryGetValue(targetType, out Type? type))
        {
            return type!;
        }
        var typeNamespace = targetType.Namespace ?? "";

        if (typeNamespace.StartsWith("System"))
        {
            return targetType;
        }
        
        if (targetType.IsArray)
        {
            RegisteredType(targetType);
            ResolveType(targetType.GetElementType()!);
            return targetType;
        }
        
        MappedType[targetType] = targetType;
        FieldInfo[] fieldInfos = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            ResolveType(fieldInfo.FieldType);
        }
        return targetType;
    }

    private static void RegisteredType(Type targetType)
    {
        if (!RegisteredTypes.Contains(targetType))
        {
            RegisteredTypes.Add(targetType);
        }
    }

    public static int GetTypeId(Type targetType)
    {
        if (!RegisteredTypes.Contains(targetType))
        {
            RegisteredType(targetType);
        }

        for (var index = 0; index < RegisteredTypes.Count; index++)
        {
            if (RegisteredTypes[index] == targetType)
            {
                return index;
            }
        }

        return -1;
    }
}