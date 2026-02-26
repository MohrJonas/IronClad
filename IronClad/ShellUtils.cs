using System.Diagnostics;

namespace Mohr.Jonas.IronClad;

public static class ShellUtils
{
    public sealed record CommandResult(int ExitCode, string Stdout, string Stderr)
    {
        public void EnsureSuccessfulExit()
        {
            if (ExitCode != 0)
                throw new Exception($"Process failed: {Stderr}");
        }
    }

    public static int RunCommandInteractively(string command, string[] args, string workingDirectory)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = command,
            Arguments = string.Join(" ", args),
            WorkingDirectory = workingDirectory
        };
        var process = Process.Start(startInfo);
        process!.WaitForExit();
        return process!.ExitCode;
    }

    public static CommandResult RunCommand(string command, string[] args, string workingDirectory)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = command,
            Arguments = string.Join(" ", args),
            WorkingDirectory = workingDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        var process = Process.Start(startInfo);
        process!.WaitForExit();
        return new(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StandardError.ReadToEnd());
    }

    public static CommandResult RunCommand(string command, string[] args) => RunCommand(command, args, Environment.CurrentDirectory);
}