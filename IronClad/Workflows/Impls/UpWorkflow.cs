using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class UpWorkflow(ILogger logger, string? cwd, string? cladConfigPath) : IWorkflow
{
    public void Run()
    {
        new BuildConfigWorkflow(logger, cwd, cladConfigPath).Run();

        var workingDirectory = cwd ?? Environment.CurrentDirectory;
        logger.LogDebug($"Working directory is '{workingDirectory}'");

        logger.LogInformation("Invoking devcontainer binary");
        var output = ShellUtils.RunCommand("devcontainer",
            ["up", "--config", ".devcontainer.json"],
            workingDirectory
        );

        logger.LogDebug($"Exitcode is '{output.ExitCode}'");
        logger.LogDebug($"Stdout is '{output.Stdout}'");
        logger.LogDebug($"Stderr is '{output.Stderr}'");
        output.EnsureSuccessfulExit();
    }
}