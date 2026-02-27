
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class RunWorkflow(ILogger logger, string? cwd, string? configPath, string[] command) : IWorkflow
{
    public void Run()
    {
        new BuildConfigWorkflow(logger, cwd, configPath).Run();

        var workingDirectory = cwd ?? Environment.CurrentDirectory;
        logger.LogDebug($"Working directory is '{workingDirectory}'");

        logger.LogDebug($"Command to run is {string.Join(" ", command)}");

        logger.LogInformation("Invoking devcontainer binary");
        var output = ShellUtils.RunCommand(
            "devcontainer",
            ["exec", "--config", ".devcontainer.json", "--", .. command],
            workingDirectory
        );

        logger.LogDebug($"Exitcode is '{output.ExitCode}'");
        logger.LogDebug($"Stdout is '{output.Stdout}'");
        logger.LogDebug($"Stderr is '{output.Stderr}'");
        output.EnsureSuccessfulExit();
    }
}