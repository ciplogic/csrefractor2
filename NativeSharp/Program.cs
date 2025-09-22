﻿using System.Reflection;
using NativeSharp.CodeGen;
using NativeSharp.Optimizations;
using NativeSharp.Resolving;

namespace NativeSharp;

internal class Program
{
    private static void Main(string[] args)
    {
        Assembly asm = Assembly.LoadFrom("TargetApp.dll");
        MethodInfo entryPoint = asm.EntryPoint!;
        AssemblyScanner.DefaultMappings();
        AssemblyScanner.ScanAssembly(typeof(Texts).Assembly);

        MethodResolver.ResolveAllTree(entryPoint);

        MethodResolver.ResolveCilMethod(MethodResolver.Resolve(entryPoint));

        MethodResolver.TransformCilMethod(typeof(Texts).GetMethod("FromIndex")!);
        MethodResolver.TransformCilMethod(typeof(Texts).GetMethod("BuildSystemString")!);

        var optimizer = new OptimizationSteps();
        optimizer.OptimizeMethodSet(MethodResolver.MethodCache.Values.ToArray());
        
        CodeGenerator codeGen = new CodeGenerator();
        codeGen.WriteMethodsAndMain(entryPoint.MangleMethodName());
        CodeGeneratorBaseTypes.GenerateNativeMappings();
    }
}