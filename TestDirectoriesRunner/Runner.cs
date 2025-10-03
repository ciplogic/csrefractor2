using System.Diagnostics;

namespace TestDirectoriesRunner;

public class Runner
{
    public static void Main(string[] args)
    {
        var startPath = @"C:\oss\csrefractor2\Tests\TestSuite\Examples\";
        var currentPath = Directory.GetCurrentDirectory();
        var directories = new DirectoryInfo(startPath).GetDirectories();
        foreach (var directory in directories)
        {
            Directory.SetCurrentDirectory(directory.FullName);
            AddCsProj(directory.FullName);
            BuildLocally();
            Console.WriteLine(directory.FullName);
        }

        Directory.SetCurrentDirectory(currentPath);
    }

    private static void BuildLocally()
    {
        Process.Start("dotnet", "build")?.WaitForExit();
        Process runProcess = new Process()
        {
            StartInfo = new ProcessStartInfo("dotnet", "run")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        };
        runProcess?.WaitForExit();
        var output = runProcess?.StandardOutput?.ReadToEnd();
        var error = runProcess?.StandardError?.ReadToEnd();
        Console.WriteLine(output);
        Console.WriteLine(error);
    }

    private static void AddCsProj(string path)
    {
        var projectContent =
            """
            <Project Sdk="Microsoft.NET.Sdk">

                <PropertyGroup>
                    <TargetFramework>net8.0</TargetFramework>
                    <LangVersion>latest</LangVersion>
                    <ImplicitUsings>enable</ImplicitUsings>
                    <Nullable>enable</Nullable>
                    <OutputType>Exe</OutputType>
                </PropertyGroup>

            </Project>
            """;
        File.WriteAllText(Path.Combine(path, "app.csproj"), projectContent);
    }
}