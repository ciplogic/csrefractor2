using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;
using NativeSharp.Optimizations;

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

        string typeNamespace = targetType.Namespace ?? "";

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
        FieldInfo[] fieldInfos =
            targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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

        for (int index = 0; index < RegisteredTypes.Count; index++)
        {
            if (RegisteredTypes[index] == targetType)
            {
                return index;
            }
        }

        return -1;
    }

    public static bool DevirtualizeCalls()
    {
        bool result = false;
        CilOperationsMethod[] cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        foreach (CilOperationsMethod cilMethod in cilMethods)
        {
            result = DevirtualizeCallsInMethod(cilMethod) || result;
        }
        return result;
    }

    public static bool DevirtualizeCallsInMethod(CilOperationsMethod cilMethod)
    {
        bool found = false;
        do
        {
            int[] virtCallsIndices = IndexOfVirtualCalls(cilMethod).ToArray();
            if (virtCallsIndices.Length == 0)
            {
                break;
            }

            foreach (int virtCallIndex in virtCallsIndices)
            {
                BaseOp op = cilMethod.Operations[virtCallIndex];
                ICallOp callOp = (ICallOp)op;
                Type declaringType = callOp.TargetMethod.DeclaringType!;
                if (IsEffectivelySealed(declaringType))
                {
                    MakeCallStatic(cilMethod, virtCallIndex);
                    Program.ApplyDefaultOptimizations(true);
                    found = true;
                    break;
                }
            }
        } while (true);

        return found;

    }

    private static bool IsEffectivelySealed(Type declaringType)
    {
        if (declaringType.IsSealed)
        {
            return true;
        }

        bool isEffectivelySealed = RegisteredTypes.Where(knownType => knownType != declaringType)
            .All(knownType => !declaringType.IsAssignableFrom(knownType));
        return isEffectivelySealed;
    }

    private static void MakeCallStatic(CilOperationsMethod cilMethod, int virtCallIndex)
    {
        BaseOp op = cilMethod.Operations[virtCallIndex];
        IVirtualCall virtualCall = (IVirtualCall)op;
        BaseOp staticOp = virtualCall.ToStatic();
        cilMethod.Operations[virtCallIndex] = staticOp;
        ICallOp callOp = (ICallOp)staticOp;
        MethodResolver.ResolveAllTree(callOp.TargetMethod);
        NativeMethodBase? cilResolved = MethodResolver.Resolve(callOp.TargetMethod);
        if (cilResolved is CilOperationsMethod resolved)
        {
            DevirtualizeCallsInMethod(resolved);
        }
    }

    private static IEnumerable<int> IndexOfVirtualCalls(CilOperationsMethod cilMethod)
    {
        for (int index = 0; index < cilMethod.Operations.Length; index++)
        {
            BaseOp op = cilMethod.Operations[index];
            if ((op is VirtualCallOp) || (op is VirtualCallReturnOp))
            {
                yield return index;
            }
        }
    }
}