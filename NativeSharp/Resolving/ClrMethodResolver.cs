using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Extensions;
using NativeSharp.Lib;
using NativeSharp.Lib.Resolvers;
using NativeSharp.Operations.Common;

namespace NativeSharp.Resolving;

static class ClrMethodResolver
{
    public static BaseNativeMethod? ResolveSystemClrMethod(MethodInfo? clrMethod)
    {
        if (clrMethod == null)
        {
            return null;
        }

        if (MethodResolver.MethodCache.TryGetValue(clrMethod, out BaseNativeMethod? method))
        {
            return method;
        }

        int parmeterCount = clrMethod.ParameterCount();


        string fullTargetMethodName = $"{clrMethod.MangleMethodName()}";
        MethodInfo[] methodInfos = typeof(ResolvedMethods)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(x => x.Name == fullTargetMethodName)
            .Where(x => x.ParameterCount() == parmeterCount)
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

        ParameterInfo[] parameterInfos = clrMethod.GetParameters();

        ParameterInfo[] mappedMethodInfo = mappedMethod.GetParameters();
        bool isStatic = clrMethod.IsStatic;
        int offset = isStatic ? 0 : 1;
        for (int i = offset; i < mappedMethodInfo.Length; i++)
        {
            ParameterInfo mappedParam = mappedMethodInfo[i];
            ParameterInfo param = parameterInfos[i];
        }

        MethodResolver.RemappedMethods[clrMethod] = mappedMethod;
        CppCodeAttribute? cppCodeAttribute = mappedMethod.GetCustomAttribute<CppCodeAttribute>();
        if (cppCodeAttribute is not null)
        {
            CppNativeMethod resolveSystemClrMethod = new(cppCodeAttribute.NativeContent)
            {
                Target = clrMethod,
                Args = clrMethod.GetMethodArguments(),
            };
            MethodResolver.MethodCache[clrMethod] = resolveSystemClrMethod;
            return resolveSystemClrMethod;
        }

        return MethodResolver.TransformCilMethod(clrMethod, mappedMethod);
    }

    private static bool AreParameterEquivalent(MethodInfo methodInfo, MethodInfo clrMethod)
    {
        ParameterInfo[] parameterInfos = clrMethod.GetParameters();

        ParameterInfo[] mappedMethodInfo = methodInfo.GetParameters();
        for (var i = 0; i < parameterInfos.Length; i++)
        {
            ParameterInfo mappedParam = mappedMethodInfo[i];
            ParameterInfo param = parameterInfos[i];
            if (mappedParam.ParameterType != param.ParameterType)
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