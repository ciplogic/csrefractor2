using System.Reflection;

namespace NativeSharp.Optimizations.IpoOptimizations;

public class TreeShaker
{
    public MethodBase[] Methods = [];
    
    public void SetEntryPointsMethods(params MethodBase[] methods)
    {
        
    }
}