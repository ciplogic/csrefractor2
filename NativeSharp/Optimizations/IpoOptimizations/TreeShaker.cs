using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.IpoOptimizations;

public class TreeShaker
{
    public MethodBase[] Methods = [];
    
    public void SetEntryPointsMethods(params MethodBase[] methods)
    {
        Methods = methods;

        HashSet<MethodBase> foundMethods = [];
        foreach (MethodBase method in Methods)
        {
            foundMethods.Add(method);
        }

        while (CanFindMore(foundMethods))
        {
            
        }
        var methodsToRemove = MethodResolver.MethodCache.Keys.Where(key => !foundMethods.Contains(key)).ToArray();
        foreach (var methodToRemove in methodsToRemove)
        {
            foundMethods.Remove(methodToRemove);
        }
    }

    private static bool CanFindMore(HashSet<MethodBase> foundMethods)
    {
        var found = false;
        var methodsToInspect = foundMethods.ToArray();
        foreach (MethodBase methodBase in methodsToInspect)
        {
            var baseNativeMethod= MethodResolver.Resolve(methodBase);
            if (baseNativeMethod is CilOperationsMethod cilMethod)
            {
                found = found || PopulateCallsFromMethod(cilMethod, foundMethods);
            }
            else
            {
                foundMethods.Add(methodBase);
            }
        }
        return found;
    }

    private static bool PopulateCallsFromMethod(CilOperationsMethod cilMethod, HashSet<MethodBase> foundMethods)
    {
        var found = false;
        foreach (var operation in cilMethod.Operations)
        {
            if (operation is ICallOp callOp)
            {
                if (!MethodResolver.RemappedMethods.TryGetValue(callOp.TargetMethod, out var mappedMethod))
                {
                    mappedMethod = callOp.TargetMethod;
                }
                found = found || foundMethods.Add(mappedMethod);
            }
        }
        return found;
    }
}