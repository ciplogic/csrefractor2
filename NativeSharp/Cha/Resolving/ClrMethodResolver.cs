using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Extensions;
using NativeSharp.Lib;
using NativeSharp.Lib.Resolvers;
using NativeSharp.Operations.Common;

namespace NativeSharp.Cha.Resolving;

internal static class ClrMethodResolver
{
    public static BaseNativeMethod? ResolveSystemClrMethod(MethodBase? clrMethod)
    {
        if (clrMethod is null)
        {
            return null;
        }

        if (MethodResolver.MethodCache.TryGetValue(clrMethod, out BaseNativeMethod? method))
        {
            return method;
        }

        if (clrMethod.IsConstructor)
        {
            return new CilOperationsMethod()
            {
                Args = clrMethod.GetMethodArguments(),
            };
        }

        int parameterCount = clrMethod.ParameterCount();


        string fullTargetMethodName = $"{clrMethod.MangleMethodName()}";
        MethodInfo[] methodInfos = typeof(ResolvedMethods)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(x => x.Name == fullTargetMethodName)
            .Where(x => x.ParameterCount() == parameterCount)
            .ToArray();
        if (methodInfos.Length == 0)
        {
            return null;
        }

        MethodInfo? mappedMethod = null;
        if (methodInfos.Length == 1)
        {
            mappedMethod = methodInfos[0];
        }
        else
        {
            foreach (MethodInfo methodInfo in methodInfos)
            {
                bool areParametersEquivalent = AreParameterEquivalent(methodInfo, clrMethod);
                if (areParametersEquivalent)
                {
                    mappedMethod = methodInfo;
                }
            }
        }

        if (mappedMethod is null)
        {
            return null;
        }

        MethodResolver.RemappedMethods[clrMethod] = mappedMethod;
        CppCodeAttribute? cppCodeAttribute = mappedMethod.GetCustomAttribute<CppCodeAttribute>();
        if (cppCodeAttribute is not null)
        {
            CppNativeMethod resolveSystemClrMethod = new(cppCodeAttribute.NativeContent)
            {
                Target = clrMethod,
                Args = mappedMethod.GetMethodArguments(),
            };
            MethodResolver.MethodCache[clrMethod] = resolveSystemClrMethod;
            return resolveSystemClrMethod;
        }

        return MethodResolver.TransformCilMethod(clrMethod, mappedMethod);
    }

    private static Type[] ExtractArgumentTypesOfMethod(this MethodBase method)
    {
        List<Type> result = new List<Type>();
        if (!method.IsStatic || method.IsConstructor)
        {
            result.Add(method.DeclaringType);
        }

        ParameterInfo[] args = method.GetParameters();
        for (int i = 0; i < args.Length; i++)
        {
            result.Add(args[i].ParameterType);
        }

        return result.ToArray();
    }

    private static bool AreParameterEquivalent(MethodInfo methodInfo, MethodBase clrMethod)
    {
        Type[] parameterInfos = clrMethod.ExtractArgumentTypesOfMethod();

        Type[] mappedMethodInfo = methodInfo.ExtractArgumentTypesOfMethod();
        for (int i = 0; i < parameterInfos.Length; i++)
        {
            Type mappedParam = mappedMethodInfo[i];
            Type param = parameterInfos[i];
            if (mappedParam != param)
            {
                return false;
            }
        }

        return true;
    }

    private static int ParameterCount(this MethodBase clrMethod)
    {
        int parmeterCount = clrMethod.GetParameters().Length;
        if (!clrMethod.IsStatic)
        {
            parmeterCount++;
        }

        return parmeterCount;
    }
}