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
        CompilerOptions options = new()
        {
            Optimize = true
        };
        if (!File.Exists("compiler_options.json"))
        {
            File.WriteAllText("compiler_options.json", JsonSerializer.Serialize(options));
        }
        else
        {
            string json = File.ReadAllText("compiler_options.json");
            options = JsonSerializer.Deserialize<CompilerOptions>(json) ?? new CompilerOptions();
        }
        
        CodeGeneratorBaseTypes.DefaultTypeMappings();

        Assembly asm = Assembly.LoadFrom(assemblyNameToScan);
        MethodInfo entryPoint = asm.EntryPoint!;
        AssemblyScanner.DefaultMappings();
        AssemblyScanner.ScanAssembly(typeof(Texts).Assembly);

        MethodResolver.ResolveAllTree(entryPoint);

        //MethodResolver.ResolveCilMethod(MethodResolver.Resolve(entryPoint));

        MethodResolver.ResolveAllTree(typeof(Texts).GetMethod("FromIndex")!);

        while (ClassHierarchyAnalysis.DevirtualizeCalls())
        {
            ApplyDefaultOptimizations(options.Optimize);
        }
        //MethodResolver.TransformCilMethod(typeof(Texts).GetMethod("BuildSystemString")!);

        ApplyDefaultOptimizations(options.Optimize);

        EscapeAnalysisStep.ApplyStaticAnalysis(EscapeAnalysisMode.Standard);
        
        TreeShaker treeShaker = new TreeShaker();
        treeShaker.SetEntryPointsMethods(entryPoint, typeof(Texts).GetMethod("FromIndex")!);

        CodeGenerator codeGen = new CodeGenerator();
        codeGen.WriteMethodsAndMain(entryPoint.MangleMethodName(), entryPoint);
        CodeGeneratorBaseTypes.GenerateNativeMappings();
        
        sw.Stop();
        Console.WriteLine($"Time to compile: {sw.Elapsed}");
    }

    public static void ApplyDefaultOptimizations(bool optimize = true)
    {
        OptimizationSteps optimizer = new OptimizationSteps(optimize);
        OptimizationSteps.OptimizeMethodSet(optimizer.CilMethodOptimizations.ToArray());
    }
}