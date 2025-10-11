namespace TestDirectoriesRunner;

public record ExecutionResult (string Output, string Error, int ExitCode);

public class Runner
{
    public static async Task Main(string[] args)
    {
        var startPath = @"C:\oss\csrefractor2\Tests\TestSuite\Examples\";
        var currentPath = Directory.GetCurrentDirectory();
        var directories = new DirectoryInfo(startPath).GetDirectories();
        foreach (var directory in directories)
        {
            Directory.SetCurrentDirectory(directory.FullName);
            AddCsProj(directory.FullName);
            await BuildLocally(directory);
            Console.WriteLine(directory.FullName);
        }

        Directory.SetCurrentDirectory(currentPath);
    }

    private static async Task BuildLocally(DirectoryInfo directory)
    {
        var resultExecute = await DotnetRunner.RunDotnetRunAsync(directory.FullName, "run App.cs");
        var (output, error) = resultExecute;
        Console.WriteLine(output);
        Console.WriteLine(error);
        if (string.IsNullOrEmpty(error))
        {
            await DotnetRunner.RunDotnetRunAsync(directory.FullName, "build");
        }
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