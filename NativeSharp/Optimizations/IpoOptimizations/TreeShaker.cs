using System.Reflection;
using NativeSharp.Cha.Resolving;
using NativeSharp.CodeGen;
using NativeSharp.CodeGen.Methods;
using NativeSharp.Operations;
using NativeSharp.Operations.Calls;
using NativeSharp.Operations.Common;

namespace NativeSharp.Optimizations.IpoOptimizations;

public class TreeShaker
{
    public MethodBase[] Methods = [];

    public void SetEntryPointsMethods(params MethodBase[] methods)
    {
        Methods = methods;

        KeyValuePair<MethodBase, NativeMethodBase>[] allMethods = MethodResolver.MethodCache.ToArray();
        SortedSet<int> usedIndices = [];
        foreach (MethodBase methodBase in methods)
        {
            usedIndices.Add(IndexOfMethod(allMethods, methodBase));
        }

        while (CanFindMoreIndices(allMethods, usedIndices))
        {
        }

        List<KeyValuePair<MethodBase, NativeMethodBase>> shackedList = [];
        foreach (int usedIndex in usedIndices)
        {
            shackedList.Add(allMethods[usedIndex]);
        }

        Dictionary<MethodBase, NativeMethodBase> dict = shackedList.ToDictionary(x => x.Key, x => x.Value);
        MethodResolver.MethodCache = dict;
    }

    private bool CanFindMoreIndices(KeyValuePair<MethodBase, NativeMethodBase>[] allMethods, SortedSet<int> usedIndices)
    {
        int[] indicesToSearch = usedIndices.ToArray();
        foreach (int index in indicesToSearch)
        {
            KeyValuePair<MethodBase, NativeMethodBase> method = allMethods[index];
            if (method.Value is CilOperationsMethod cilMethod)
            {
                foreach (BaseOp baseOp in cilMethod.Operations)
                {
                    if (baseOp is ICallOp callOp)
                    {
                        int indexOfMethodCall = IndexOfMethod(allMethods, callOp.Resolved);
                        if (indexOfMethodCall != -1)
                        {
                            usedIndices.Add(indexOfMethodCall);
                        }
                    }
                }
            }
        }

        return indicesToSearch.Length != usedIndices.Count;
    }

    private static int IndexOfMethod(KeyValuePair<MethodBase, NativeMethodBase>[] allMethods, MethodBase method)
    {
        for (int index = 0; index < allMethods.Length; index++)
        {
            KeyValuePair<MethodBase, NativeMethodBase> methodToRemove = allMethods[index];
            if (method == (methodToRemove.Value.Target))
            {
                return index;
            }
        }

        return -1;
    }

    private static int IndexOfMethod(KeyValuePair<MethodBase, NativeMethodBase>[] allMethods, NativeMethodBase method)
    {
        for (int index = 0; index < allMethods.Length; index++)
        {
            KeyValuePair<MethodBase, NativeMethodBase> methodToRemove = allMethods[index];
            if (method.MangledMethodHeader() == (methodToRemove.Value.MangledMethodHeader()))
            {
                return index;
            }
        }

        return -1;
    }
}