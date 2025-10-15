namespace TestDirectoriesRunner;

public record ExecutionResult (string Output, string Error, int ExitCode);

public class Runner
{
    private const string SolutionPath = @"C:\oss\csrefractor2";
    
    public static async Task Main(string[] args)
    {
        var startPath = $@"{SolutionPath}\Tests\TestSuite\Examples\";
        var currentPath = Directory.GetCurrentDirectory();
        var directories = new DirectoryInfo(startPath).GetDirectories();
        foreach (var directory in directories)
        {
            Directory.SetCurrentDirectory(directory.FullName);
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
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Error: {error} when building directory: {directory.FullName}");
            return;
        }

        await AddCsProj(directory.FullName);
        await DotnetRunner.RunDotnetRunAsync(directory.FullName, "build");
        
    }

    private static async Task AddCsProj(string path)
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
        await File.WriteAllTextAsync(Path.Combine(path, "app.csproj"), projectContent);
    }
}