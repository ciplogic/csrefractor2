using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Common;
using NativeSharp.FrontEnd;
using NativeSharp.Lib.Resolvers;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;

namespace NativeSharp.Cha.Resolving;

internal static class MethodResolver
{
    public static Dictionary<MethodBase, NativeMethodBase> MethodCache { get; set; } = [];
    public static Dictionary<MethodBase, MethodBase> RemappedMethods { get; } = [];

    public static List<IMethodResolver> AllMethodResolvers { get; } = [];


    public static NativeMethodBase? Resolve(MethodBase clrMethod)
    {
        try
        {
            string signature = clrMethod.MangleMethodName();
            if (signature.StartsWith("System"))
            {
                NativeMethodBase? systemClrMethod = ClrMethodResolver.ResolveSystemClrMethod(clrMethod);
                if (systemClrMethod != null)
                {
                    systemClrMethod.Target = clrMethod;
                    TransformCilMethod(clrMethod, clrMethod);
                }

                return systemClrMethod;
            }

            ClassHierarchyAnalysis.ResolveType(clrMethod.DeclaringType);
            return TransformCilMethod(clrMethod, clrMethod);
        }
        catch
        {
            return null!;
        }
    }

    public static NativeMethodBase? TransformCilMethod(MethodBase clrMethod, MethodBase? remappedClrMethod = null)
    {
        remappedClrMethod ??= clrMethod;
        InstructionTransformer transformer = new InstructionTransformer();
        CilOperationsMethod transformCilMethod = new ()
        {
            Target = clrMethod,
        };
        MethodCache[clrMethod] = transformCilMethod;
        BaseOp[] operations = transformer.Transform(remappedClrMethod);
        transformCilMethod.Locals = transformer.LocalVariablesStackAndState.LocalVariables.ToArray();
        transformCilMethod.Args = transformer.Params.ToArray();
        transformCilMethod.Operations = operations;
        ResolveCallsOfMethods(transformCilMethod);
        return transformCilMethod;
    }

    private static AnalysisProgress ResolveCallsOfMethods(CilOperationsMethod transformCilMethod)
    {
        if (transformCilMethod.Analysis.CallsResolved == AnalysisProgress.Done)
        {
            return AnalysisProgress.Done;
        }
        AnalysisProgress result = AnalysisProgress.Done;
        ICallOp[] callTargets = transformCilMethod.Operations.OfType<ICallOp>().ToArray();
        foreach (ICallOp callTarget in callTargets)
        {
            if (callTarget.Resolved is not UnresolvedMethod unresolvedMethod)
            {
                continue;
            }
            if (!MethodCache.TryGetValue(unresolvedMethod.Target, out NativeMethodBase? resolvedMethod))
            {
                NativeMethodBase? resolved = Resolve(callTarget.TargetMethod);
                if (resolved is not null)
                {
                    if (resolved is CilOperationsMethod resolveCilMethod)
                    {
                        ResolveCallsOfMethods(resolveCilMethod);
                    }
                    continue;
                }
                result = AnalysisProgress.NotDone;
                continue;
            }
            callTarget.Resolved = resolvedMethod;
            if (resolvedMethod is CilOperationsMethod resolvedCilMethod)
            {
                ResolveCallsOfMethods(resolvedCilMethod);
            }
        }

        return result;
    }

    public static void ResolveMethod(MethodBase clrMethod)
    {
        if (MethodCache.ContainsKey(clrMethod))
        {
            return;
        }

        ResolveCilMethod(ClrMethodResolver.ResolveSystemClrMethod(clrMethod));
    }

    public static void ResolveCilMethod(NativeMethodBase? method)
    {
        if (method is not CilOperationsMethod cilMethod)
        {
            return;
        }

        ICallOp[] callTargets = cilMethod.Operations.OfType<ICallOp>().ToArray();
        foreach (ICallOp target in callTargets)
        {
            NativeMethodBase? resolved = Resolve(target.TargetMethod);
            if (resolved is not null)
            {
                MethodCache[target.TargetMethod] = resolved;
            }
        }
        cilMethod.Analysis.CallsResolved = ResolveCallsOfMethods(cilMethod);
    }

    public static void ResolveAllTree(MethodBase? entryPoint)
    {
        if (entryPoint is null)
        {
            return;
        }
        if (MethodCache.ContainsKey(entryPoint))
        {
            return;
        }
        
        NativeMethodBase? resolvedMethod = Resolve(entryPoint);
        if (resolvedMethod is not CilOperationsMethod cilMethod)
        {
            return;
        }
        
        ICallOp[] methodCalls = cilMethod.Operations.OfType<ICallOp>().ToArray();
        
        foreach (ICallOp method in methodCalls)
        {
            if (method.Resolved is  UnresolvedMethod unresolvedMethod)
            {
                ResolveAllTree(unresolvedMethod.Target);    
            }
            
        }
        
        ResolveCallsOfMethods(cilMethod);
        
    }
}