using System.Diagnostics;

public static class DotnetRunner
{
    // Runs 'dotnet run' in the specified working directory and returns (Output, Error)
    public static async Task<(string Output, string Error)> RunDotnetRunAsync(string workingDirectory, string arguments)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = process.StandardError.ReadToEndAsync();

            // Wait for both streams to finish and the process to exit
            await Task.WhenAll(outputTask, errorTask);
            await process.WaitForExitAsync();

            string output = await outputTask;
            string error = await errorTask;

            return (output, error);
        }
        catch (Exception ex)
        {
            // If the process couldn't start or another error occurred
            return (string.Empty, ex.ToString());
        }
    }

    public static string[] SplitOutputIntoLines(this string output)
    {
        var lines = new List<string>();
        using StringReader reader = new StringReader(output);
        while (true)
        {
            string? line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            lines.Add(line);
                
        }

        return lines.ToArray();
    }
}