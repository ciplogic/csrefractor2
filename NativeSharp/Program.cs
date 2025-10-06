using System.Reflection;
using System.Text.Json;
using NativeSharp.Cha;
using NativeSharp.Cha.Resolving;
using NativeSharp.CodeGen;
using NativeSharp.Common;
using NativeSharp.EscapeAnalysis;
using NativeSharp.Optimizations;

namespace NativeSharp;

internal class Program
{
    private static void Main(string[] args)
    {
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

        Assembly asm = Assembly.LoadFrom("TargetApp.dll");
        MethodInfo entryPoint = asm.EntryPoint!;
        AssemblyScanner.DefaultMappings();
        AssemblyScanner.ScanAssembly(typeof(Texts).Assembly);

        MethodResolver.ResolveAllTree(entryPoint);

        MethodResolver.ResolveCilMethod(MethodResolver.Resolve(entryPoint));

        MethodResolver.ResolveAllTree(typeof(Texts).GetMethod("FromIndex")!);
        
        ClassHierarchyAnalysis.DevirtualizeCalls();
        //MethodResolver.TransformCilMethod(typeof(Texts).GetMethod("BuildSystemString")!);

        OptimizationSteps optimizer = new OptimizationSteps(options.Optimize);
        OptimizationSteps.OptimizeMethodSet(optimizer.CilMethodOptimizations.ToArray());

        EscapeAnalysisStep.ApplyStaticAnalysis();

        CodeGenerator codeGen = new CodeGenerator();
        codeGen.WriteMethodsAndMain(entryPoint.MangleMethodName(), entryPoint);
        CodeGeneratorBaseTypes.GenerateNativeMappings();
    }
}