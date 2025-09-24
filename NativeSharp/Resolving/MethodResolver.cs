using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Common;
using NativeSharp.FrontEnd;
using NativeSharp.Lib.Resolvers;
using NativeSharp.Operations;
using NativeSharp.Operations.Common;

namespace NativeSharp.Resolving;

internal static class MethodResolver
{
    public static Dictionary<MethodBase, BaseNativeMethod> MethodCache { get; } = [];
    public static Dictionary<MethodBase, MethodBase> RemappedMethods { get; } = [];
    public static TwoWayDictionary<Type> MappedType { get; } = new();

    public static List<IMethodResolver> AllMethodResolvers { get; } = [];


    public static Type ResolveType(Type targetType)
    {
        if (targetType.IsArray)
        {
            ResolveType(targetType.GetElementType()!);
            return targetType;
        }
        if (MappedType.TryGetValue(targetType, out Type? type))
        {
            return type;
        }
        var typeNamespace = targetType.Namespace ?? "";

        if (typeNamespace.StartsWith("System"))
        {
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

    public static BaseNativeMethod? Resolve(MethodBase clrMethod)
    {
        Type declaringType = clrMethod.DeclaringType!;
        string signature = clrMethod.MangleMethodName();
        if (signature.StartsWith("System"))
        {
            BaseNativeMethod? systemClrMethod = ClrMethodResolver.ResolveSystemClrMethod(clrMethod);
            if (systemClrMethod != null)
            {
                systemClrMethod.Target = clrMethod;
                TransformCilMethod(clrMethod, clrMethod);
            }

            return systemClrMethod;
        }
        ResolveType(clrMethod.DeclaringType);
        return TransformCilMethod(clrMethod, clrMethod);
    }

    public static BaseNativeMethod? TransformCilMethod(MethodBase clrMethod, MethodBase? remappedClrMethod = null)
    {
        remappedClrMethod ??= clrMethod;
        InstructionTransformer transformer = new InstructionTransformer();
        CilNativeMethod transformCilMethod = new CilNativeMethod()
        {
            Target = clrMethod,
        };
        MethodCache[clrMethod] = transformCilMethod;
        BaseOp[] operations = transformer.Transform(remappedClrMethod);
        transformCilMethod.Locals = transformer.LocalVariablesStackAndState.LocalVariables.ToArray();
        transformCilMethod.Args = transformer.Params.ToArray();
        transformCilMethod.Instructions = operations;

        return transformCilMethod;
    }

    public static void ResolveMethod(MethodBase clrMethod)
    {
        if (MethodCache.ContainsKey(clrMethod))
        {
            return;
        }

        ResolveCilMethod(ClrMethodResolver.ResolveSystemClrMethod(clrMethod as MethodInfo));
    }

    public static void ResolveCilMethod(BaseNativeMethod? method)
    {
        if (method is not CilNativeMethod cilMethod)
        {
            return;
        }

        MethodCache.TryAdd(cilMethod.Target, cilMethod);

        MethodBase[] callTargets = cilMethod.Instructions.OfType<CallOp>().Select(x => x.TargetMethod).ToArray();
        foreach (MethodBase target in callTargets)
        {
            BaseNativeMethod? resolved = Resolve(target);
            if (resolved is not null)
            {
                MethodCache[target] = resolved!;
            }
        }
    }

    public static void ResolveAllTree(MethodBase entryPoint)
    {
        if (MethodCache.ContainsKey(entryPoint))
        {
            return;
        }
        
        var resolvedMethod = Resolve(entryPoint);
        if (resolvedMethod is not CilNativeMethod cilMethod)
        {
            return;
        }
        var methodsVoid = cilMethod.Instructions.OfType<CallOp>().Select(x=>x.TargetMethod).ToArray();
        var methodsReturn = cilMethod.Instructions.OfType<CallReturnOp>().Select(x => x.TargetMethod).ToArray();
        var joinedMethodsToResolve = methodsVoid.Concat(methodsReturn).ToArray();
        if (joinedMethodsToResolve.Length == 0)
        {
            return;
        }

        foreach (MethodBase method in joinedMethodsToResolve)
        {
            ResolveAllTree(method);
        }
        
    }
}