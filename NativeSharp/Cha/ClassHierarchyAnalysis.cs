using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;

namespace NativeSharp.Cha;

public static class ClassHierarchyAnalysis
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

    public static void DevirtualizeCalls()
    {
        var cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        foreach (var cilMethod in cilMethods)
        {
            DevirtualizeCallsInMethod(cilMethod);
        }
    }

    public static void DevirtualizeCallsInMethod(CilOperationsMethod cilMethod)
    {
        var virtCallsIndices = IndexOfVirtualCalls(cilMethod).ToArray();
        foreach (var virtCallIndex in virtCallsIndices)
        {
            var op = cilMethod.Operations[virtCallIndex];
            var callOp = (ICallOp)op;
            var declaringType = callOp.TargetMethod.DeclaringType!;
            if (IsEffectivelySealed(declaringType))
            {
                MakeCallStatic(cilMethod, virtCallIndex);
                continue;
            }
        }
    }

    private static bool IsEffectivelySealed(Type declaringType)
    {
        if (declaringType.IsSealed)
        {
            return true;
        }

        return RegisteredTypes.Where(knownType => knownType != declaringType).All(knownType => !declaringType.IsAssignableFrom(knownType));
    }

    private static void MakeCallStatic(CilOperationsMethod cilMethod, int virtCallIndex)
    {
        var op = cilMethod.Operations[virtCallIndex];
        var virtualCall= (IVirtualCall)op;
        var staticOp = virtualCall.ToStatic();
        cilMethod.Operations[virtCallIndex] = staticOp;
        var callOp = (ICallOp)staticOp;
         MethodResolver.ResolveAllTree(callOp.TargetMethod);
         var cilResolved = MethodResolver.Resolve(callOp.TargetMethod);
         if (cilResolved is CilOperationsMethod resolved)
         DevirtualizeCallsInMethod(resolved);
    }

    static IEnumerable<int> IndexOfVirtualCalls(CilOperationsMethod cilMethod)
    {
        for (var index = 0; index < cilMethod.Operations.Length; index++)
        {
            var op = cilMethod.Operations[index];
            if ((op is VirtualCallOp ) ||(op is VirtualCallReturnOp))
            {
                yield return index;
            }
        }
    }
}