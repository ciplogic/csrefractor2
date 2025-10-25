using System.Reflection;
using NativeSharp.Common;
using NativeSharp.Extensions;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;

namespace NativeSharp.Cha;

public static class ClassHierarchyAnalysis
{
    public static OrderedList<Type> RegisteredTypes { get; } = new();
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
        => RegisteredTypes.Set(targetType);

    public static int GetTypeId(Type targetType) 
        => RegisteredTypes.Set(targetType);

    public static bool DevirtualizeCalls(OptimizationOptions optimizationOptions)
    {
        bool result = false;
        CilOperationsMethod[] cilMethods = CilNativeMethodExtensions.CilMethodsFromCache();
        foreach (CilOperationsMethod cilMethod in cilMethods)
        {
            result = DevirtualizeCallsInMethod(cilMethod, optimizationOptions) || result;
        }
        return result;
    }

    public static bool DevirtualizeCallsInMethod(CilOperationsMethod cilMethod, OptimizationOptions optimizationOptions)
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
                    Program.ApplyDefaultOptimizations(optimizationOptions);
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

        bool isEffectivelySealed = RegisteredTypes
            .Items
            .Where(knownType => knownType != declaringType)
            .All(knownType => !declaringType.IsAssignableFrom(knownType));
        return isEffectivelySealed;
    }

    private static void MakeCallStatic(CilOperationsMethod cilMethod, int virtCallIndex)
    {
        BaseOp[] operations = cilMethod.Operations;
        BaseOp op = operations[virtCallIndex];
        IVirtualCall virtualCall = (IVirtualCall)op;
        BaseOp staticOp = virtualCall.ToStatic();
        operations[virtCallIndex] = staticOp;
    }

    private static IEnumerable<int> IndexOfVirtualCalls(CilOperationsMethod cilMethod)
    {
        for (int index = 0; index < cilMethod.Operations.Length; index++)
        {
            BaseOp op = cilMethod.Operations[index];
            if (op is VirtualCallOp or VirtualCallReturnOp)
            {
                yield return index;
            }
        }
    }
}