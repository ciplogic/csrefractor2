using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using NativeSharp.Cha;
using NativeSharp.Cha.Resolving;
using NativeSharp.CodeGen;
using NativeSharp.Common;
using NativeSharp.EscapeAnalysis;
using NativeSharp.Optimizations;
using NativeSharp.Optimizations.IpoOptimizations;

namespace NativeSharp;

internal class Program
{
    private static void Main(string[] args)
    {
        string assemblyNameToScan = "TargetApp.dll";
        if (args.Length > 0)
        {
            assemblyNameToScan = args[0];
        }
        Stopwatch sw = Stopwatch.StartNew();
        var options = ReadCompilerOptions();

        CodeGeneratorBaseTypes.DefaultTypeMappings();

        Assembly asm = Assembly.LoadFrom(assemblyNameToScan);
        MethodInfo entryPoint = asm.EntryPoint!;
        AssemblyScanner.DefaultMappings();
        AssemblyScanner.ScanAssembly(typeof(Texts).Assembly);

        MethodResolver.ResolveAllTree(entryPoint);

        //MethodResolver.ResolveCilMethod(MethodResolver.Resolve(entryPoint));

        MethodResolver.ResolveAllTree(typeof(Texts).GetMethod("FromIndex")!);

        while (ClassHierarchyAnalysis.DevirtualizeCalls(options.Optimize))
        {
            ApplyDefaultOptimizations(options.Optimize);
        }
        //MethodResolver.TransformCilMethod(typeof(Texts).GetMethod("BuildSystemString")!);

        ApplyDefaultOptimizations(options.Optimize);

        EscapeAnalysisStep.ApplyStaticAnalysis(options.Optimize.EscapeAnalysisMode);
        
        TreeShaker treeShaker = new TreeShaker();
        treeShaker.SetEntryPointsMethods(entryPoint, typeof(Texts).GetMethod("FromIndex")!);

        CodeGenerator codeGen = new CodeGenerator();
        codeGen.WriteMethodsAndMain(entryPoint.MangleMethodName(), entryPoint, options.Optimize.EscapeAnalysisMode);
        CodeGeneratorBaseTypes.GenerateNativeMappings();
        
        sw.Stop();
        Console.WriteLine($"Time to compile: {sw.Elapsed}");
    }

    private static CompilerOptions ReadCompilerOptions()
    {
        CompilerOptions options = new()
        {
            Optimize =
            {
                UseFieldDeduplication = true,
                UseInlining = true,
                EscapeAnalysisMode = EscapeAnalysisMode.Standard
            }
        };

        if (File.Exists("compiler_options.json"))
        {
            string json = File.ReadAllText("compiler_options.json");
            //options = JsonSerializer.Deserialize<CompilerOptions>(json) ?? new CompilerOptions();
        }

        File.WriteAllText("compiler_options.json", JsonSerializer.Serialize(options));

        return options;
    }

    public static void ApplyDefaultOptimizations(OptimizationOptions optimizationOptions)
    {
        OptimizationSteps optimizer = new OptimizationSteps(optimizationOptions);
        OptimizationSteps.OptimizeMethodSet(optimizer.CilMethodOptimizations.ToArray());
    }
}